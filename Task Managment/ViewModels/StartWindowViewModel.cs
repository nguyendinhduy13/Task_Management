using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Task_Managment.DataAccess;
using Task_Managment.Models;
using Task_Managment.Views;
using System.Configuration;

namespace Task_Managment.ViewModels
{
    public class StartWindowViewModel
    {
        private MemberDataAccess db = MemberDataAccess.Instance;
        private Register mRegisterHandler = new Register();

        public string mLoginEmail { get; set; }
        public string mRegisterEmail { get; set; }
        public string mRegisterUsername { get; set; }
        public bool mIsUserRemember { get; set; }

        private static Members mCurrentUser;
        public static bool mIsUser;

        public Action CloseAction { get; set; }
        public Action HideAction { get; set; }
        public Action ShowAction { get; set; }

        public StartWindowViewModel()
        {
            Initialize();
        }

        #region Commands
        public ICommand LoginAsUserCmd { get; set; }
        public ICommand LoginAsGuestCmd { get; set; }
        public ICommand RegisterNewAccountCmd { get; set; }
        public ICommand ResetPasswordCmd { get; set; }
        #endregion

        #region Functions
        private void Initialize()
        {
            InitCommands();

            //IsUserLoggedIn();
        }

        public bool IsUserLoggedIn() {
            if (Properties.Settings.Default.isLoggedIn)
            {
                MainWindow mainWindow = new MainWindow();
                CloseAction.Invoke();
                mainWindow.Show();
            }

            return true;
        }

        public Members getCurrentUser() { return mCurrentUser; }

        private void InitCommands()
        {
            LoginAsUserCmd = new RelayCommand<PasswordBox>(p => true, p => LoginAsUser(p));
            LoginAsGuestCmd = new RelayCommand<Button>(p => true, p => LoginAsGuest());
            RegisterNewAccountCmd = new RelayCommand<object>(p => true, p => CreateNewAccount(p));
            ResetPasswordCmd = new RelayCommand<Button>(p => true, p => ResetPassword());
        }

        private void ResetPassword()
        {
            if (mLoginEmail == null)
            {
                MessageBox.Show("Please enter your email for reset password!");
                return;
            }

            try
            {
                string verificationCode = mRegisterHandler.GenerateVerficationCode();
                mRegisterHandler.SendVerificationCode(verificationCode, mLoginEmail);

                wndVerifyCode newWindow = new wndVerifyCode(verificationCode);
                newWindow.ShowDialog();
                if (newWindow.DialogResult == false)
                {
                    MessageBox.Show("Wrong verification code!");
                    return;
                }
                else
                {
                    List<Members> resetPasswordMember = db.GetMemberWithEmail(mLoginEmail);

                    if (resetPasswordMember.Count > 0) resetPasswordMember[0].Password = "1";

                    db.ResetMemberPassword(resetPasswordMember[0]);

                    MessageBox.Show("Your password has been reset to '1'!");

                    return;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateNewAccount(object p)
        {
            var values = (object[])p;
            PasswordBox registerPassword = (PasswordBox)values[0];
            PasswordBox confirmPassword = (PasswordBox)values[1];

            bool isValid = false;

            mRegisterHandler.isValidatePatternCheck(mRegisterEmail,ref isValid); if (!isValid) return;
            mRegisterHandler.isExistedCheck(mRegisterEmail, ref isValid); if (!isValid) return;
            mRegisterHandler.isPasswordValid(registerPassword.Password, confirmPassword.Password, ref isValid); if (!isValid) return;

            try
            {
                string verificationCode = mRegisterHandler.GenerateVerficationCode();

                mRegisterHandler.SendVerificationCode(verificationCode, mRegisterEmail);

                wndVerifyCode newWindow = new wndVerifyCode(verificationCode);
                newWindow.ShowDialog();
                if (newWindow.DialogResult == false)
                {
                    MessageBox.Show("Wrong verification code!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                mRegisterHandler.AddNewMember(mRegisterEmail, mRegisterUsername, registerPassword.Password);

                mCurrentUser = new Members(mRegisterEmail, mRegisterUsername, registerPassword.Password);
                mIsUser = true;

                //MessageBox.Show("Successfully sign up!");
                //MainWindow newWindow = new MainWindow();
                //newWindow.Show();
                //CloseAction.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoginAsGuest()
        {
            mCurrentUser = new Members("guest@gmail.com","Guest","");
            mIsUser = false;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            CloseAction.Invoke();
        }

        private void LoginAsUser(PasswordBox p)
        {
            string password = p.Password.ToString();
            mIsUser = true;
            List<Members> members = db.GetMemberWithEmailAndPassword(mLoginEmail, password);


            if (members != null && members.Count == 1)
            {
                mCurrentUser = members[0];
                mIsUser = true;

                if (mIsUserRemember)
                {
                    Properties.Settings.Default["UserEmail"] = mCurrentUser.Email;
                    Properties.Settings.Default["UserName"] = mCurrentUser.UserName;
                    Properties.Settings.Default["UserPassword"] = mCurrentUser.Password;

                    Properties.Settings.Default.isUser = true;
                    Properties.Settings.Default.isLoggedIn = true;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }

                MainWindow mainWindow = new MainWindow();
                CloseAction.Invoke();
                mainWindow.Show();
            }
            else
            {
                System.Windows.MessageBox.Show("Wrong email or password!");
            }
        }
        #endregion
    }
}
