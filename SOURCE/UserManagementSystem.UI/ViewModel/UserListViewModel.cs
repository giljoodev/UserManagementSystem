using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UserManagementSystem.Database;
using UserManagementSystem.Database.Entity;
using UserManagementSystem.UI.Method;
using UserManagementSystem.UI.Object;

namespace UserManagementSystem.UI.ViewModel
{
    public class UserListViewModel : INotifyPropertyChanged
    {
        #region public 선언부
        public ILogger Logger { get; set; } = new SerilogWrapper().GetLogger("UI.log");

        public ICommand SearchCommand     { get; set; }
        public ICommand AddCommand        { get; set; }
        public ICommand LeftDoubleCommand { get; set; }
        public ICommand RemoveCommand     { get; set; }
        public ICommand UpdateCommand     { get; set; }
        public ICommand InsertCommand     { get; set; }

        public ComboBoxItem ComboBoxSelectItem    { get { return _comboBoxSelectItem;    } set { _comboBoxSelectItem    = value; OnPropertyChanged(); } }
        public List<User>    UserList             { get { return _userList;             } set { _userList             = value; OnPropertyChanged(); } }
        public string       SelectText            { get { return _selectText;            } set { _selectText            = value; OnPropertyChanged(); } }
        public User         UserInfo              { get { return _userInfo;              } set { _userInfo              = value; OnPropertyChanged(); } }
        public Visibility   UserDataViewVisbility { get { return _userDataViewVisbility; } set { _userDataViewVisbility = value; OnPropertyChanged(); } }
        public Visibility   UpdateVisibility      { get { return _updateVisibility;      } set { _updateVisibility      = value; OnPropertyChanged(); } }
        public Visibility   InsertVisibility      { get { return _insertVisibility;      } set { _insertVisibility      = value; OnPropertyChanged(); } }
        public bool         IsNotRuning           { get { return _isNotRuning;           } set { _isNotRuning           = value; OnPropertyChanged(); } } //데이터 삽입 삭제 갱신 시 처리 중인 데이터에 접근하는 것을 막기 위해 사용하는 flag 변수
        #endregion

        #region private 선언부
        // lock문에 사용될 객체
        private object       _lockObject = new object();
        private ComboBoxItem _comboBoxSelectItem = new ComboBoxItem() { Content = "Name" };
        private List<User>   _userList;
        private User?        _userInfo;
        private string       _selectText;
        private Visibility   _userDataViewVisbility;
        private Visibility   _updateVisibility;
        private Visibility   _insertVisibility;
        private bool         _isNotRuning;
        #endregion

        public UserListViewModel()
        {

            SearchCommand     = new GenerateThreadCommand(SearchData, _lockObject);
            UpdateCommand     = new GenerateThreadCommand(UpdateUserData, _lockObject);
            InsertCommand     = new GenerateThreadCommand(InsertUserData, _lockObject);
            AddCommand        = new Command(CreateUserData);
            LeftDoubleCommand = new ParameterizedCommand(ChoiceUserData);
            RemoveCommand     = new ParameterizedCommand(RemoveUserData);

            IsNotRuning = true;

            //전체 검색결과 조회
            SearchData();
        }

        // User 테이블 검색 기능
        private void SearchData()
        {
            try
            {
                UserDataViewVisbility = Visibility.Collapsed;
                Thread.Sleep(1000);
                using (var context = new DatabaseContext())
                {
                    // Dispatcher를 사용하여 UI 쓰레드가 점유하고 있는 객체 접근
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        //조건에 따라 Table 탐색 조건 변경
                        switch (ComboBoxSelectItem.Content.ToString())
                        {
                            case "Name":
                                UserList = SelectText is null ? context.TblUser.ToList() : context.TblUser.Where(x => x.Name.Contains(SelectText)).ToList();
                                break;
                            case "PhoneNumber":
                                UserList = SelectText is null ? context.TblUser.ToList() : context.TblUser.Where(x => x.PhoneNumber.Contains(SelectText)).ToList();
                                break;
                            default:
                                break;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        private void RemoveUserData(object selectedItem)
        {
            IsNotRuning = false;
            OnPropertyChanged();

            try
            {
                Thread.Sleep(1000);
                var dataRowView = selectedItem as DataRowView;

                using (var context = new DatabaseContext())
                {
                    var userInfo = context.TblUser.FirstOrDefault(x => x.Index == (int)dataRowView.Row.ItemArray[0]);

                    if (userInfo != null)
                    {
                        context.TblUser.Remove(userInfo);
                        context.SaveChanges();
                    }
                    else //선택한 데이터를 검색하지 못할 경우 처리
                    {
                        MessageBox.Show("이미 삭제되었거나 찾을 수 없는 데이터입니다.");
                        return;
                    }
                }

                MessageBox.Show("User 정보를 삭제했습니다.");
                SearchData();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            finally
            {
                IsNotRuning = true;
                OnPropertyChanged();
            }
        }

        private void CreateUserData()
        {
            try
            {
                UserInfo = new User();

                UserDataViewVisbility = Visibility.Visible;
                InsertVisibility      = Visibility.Visible;
                UpdateVisibility      = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        private void InsertUserData()
        {
            IsNotRuning = false;
            OnPropertyChanged();

            try
            {
                if (UserValidate(UserInfo))
                {
                    Thread.Sleep(1000);
                    using (var context = new DatabaseContext())
                    {
                        context.TblUser.Add(UserInfo);
                        context.SaveChanges();
                    }

                    MessageBox.Show("User 정보가 추가했습니다.");
                    SearchData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("User 정보가 올바르지 않아 추가 할 수 없습니다.");
                Logger.Error(ex.ToString());
            }
            finally
            {
                IsNotRuning = true;
                OnPropertyChanged();
            }
        }

        private void ChoiceUserData(object selectedItem)
        {
            try
            {
                User userData = selectedItem as User;

                if (userData != null)
                {
                    UserInfo = new User()
                    {
                        Index       = userData.Index,
                        Name        = userData.Name,
                        Age         = userData.Age,
                        PhoneNumber = userData.PhoneNumber,
                        IsInit      = userData.IsInit,
                    };

                    UserDataViewVisbility = Visibility.Visible;
                    UpdateVisibility      = Visibility.Visible;
                    InsertVisibility      = Visibility.Collapsed;

                    OnPropertyChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        //Update 기능 Task 비동기로 구현
        private async void UpdateUserData()
        {
            IsNotRuning = false;
            OnPropertyChanged();

            try
            {
                if (UserValidate(UserInfo))
                {
                    Task.Delay(1000).Wait();

                    await Task.Run(() => {
                        using (var context = new DatabaseContext())
                        {
                            context.TblUser.Update(UserInfo);
                            context.SaveChanges();
                        }
                    });

                    MessageBox.Show("User 정보를 갱신했습니다.");
                    SearchData();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            finally
            {
                IsNotRuning = true;
                OnPropertyChanged();
            }
        }

        private bool UserValidate(User user)
        {
            var validationContext = new ValidationContext(user, null, null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            if (isValid)
            {
                return true;
            }
            else
            {
                string errorMessage = string.Empty;

                foreach (var validationResult in validationResults)
                {
                    errorMessage += validationResult.ErrorMessage + "\n";
                }

                MessageBox.Show(errorMessage);
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
