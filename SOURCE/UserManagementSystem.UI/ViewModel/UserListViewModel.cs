using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
        public DataTable    UserTable             { get { return _userTable;             } set { _userTable             = value; OnPropertyChanged(); } }
        public string       SelectText            { get { return _selectText;            } set { _selectText            = value; OnPropertyChanged(); } }
        public User         UserInfo              { get { return _userInfo;              } set { _userInfo              = value; OnPropertyChanged(); } }
        public Visibility   UserDataViewVisbility { get { return _userDataViewVisbility; } set { _userDataViewVisbility = value; OnPropertyChanged(); } }
        public Visibility   UpdateVisibility      { get { return _updateVisibility;      } set { _updateVisibility      = value; OnPropertyChanged(); } }
        public Visibility   InsertVisibility      { get { return _insertVisibility;      } set { _insertVisibility      = value; OnPropertyChanged(); } }
        public bool         IsNotRuning           { get { return _isNotRuning;           } set { _isNotRuning           = value; OnPropertyChanged(); } } //데이터 삽입 삭제 갱신 시 처리 중인 데이터에 접근하는 것을 막기 위해 사용하는 flag 변수

        public string Name
        {
            get { return _name; }
            set
            {
                //TextBox 내용 초기화 시 사용
                if (value == string.Empty)
                {
                    _name = value;
                    OnPropertyChanged();
                    return;
                }

                //한국 이름 관련 정규표현식 작성
                Regex regex = new Regex(@"^[가-힣]{1,5}$");

                Match m = regex.Match(value);

                if (!m.Success)
                {
                    MessageBox.Show("이름을 다시 입력해주세요");
                    _name = string.Empty;
                }
                else
                {
                    _name = value;
                    UserInfo.Name = _name;
                }

                OnPropertyChanged();
            }
        }

        public string Age
        {
            get { return _age; }
            set
            {
                //TextBox 내용 초기화 시 사용
                if (value == string.Empty)
                {
                    _age = value;
                    OnPropertyChanged();
                    return;
                }

                //나이 관련 정규표현식 작성
                Regex regex = new Regex(@"[0-9]{1,3}");

                Match m = regex.Match(value);

                if (!m.Success)
                {
                    MessageBox.Show("나이를 다시 입력해주세요");
                    _age = string.Empty;
                }
                else
                {
                    _age = value;
                    UserInfo.Age = _age == string.Empty ? (short)0 : short.Parse(_age);
                }
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; } 
            set
            {
                //TextBox 내용 초기화 시 사용
                if (value == string.Empty)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                    return;
                }

                //휴대폰 번호 관련 정규표현식 작성
                Regex regex = new Regex(@"01{1}[016789]{1}[0-9]{3,4}[0-9]{4}");

                Match m = regex.Match(value);

                if (!m.Success)
                {
                    MessageBox.Show("전화번호를 다시 입력해주세요");
                    _phoneNumber = string.Empty;
                }
                else
                {
                    _phoneNumber = value;
                    UserInfo.PhoneNumber = _phoneNumber;
                }

                OnPropertyChanged();
            }
        }
        #endregion

        #region private 선언부
        // lock문에 사용될 객체
        private object       _lockObject = new object();
        private ComboBoxItem _comboBoxSelectItem = new ComboBoxItem() { Content = "Name" };
        private DataTable    _userTable;
        private User?        _userInfo;
        private string       _name;
        private string       _age;
        private string       _phoneNumber;
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
                                UserTable = SelectText is null ? context.TblUser.ToList().ToDataTable() : context.TblUser.Where(x => x.Name.Contains(SelectText)).ToList().ToDataTable();
                                break;
                            case "PhoneNumber":
                                UserTable = SelectText is null ? context.TblUser.ToList().ToDataTable() : context.TblUser.Where(x => x.PhoneNumber.Contains(SelectText)).ToList().ToDataTable();
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

                Name        = string.Empty;
                Age         = string.Empty;
                PhoneNumber = string.Empty;

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
                Thread.Sleep(1000);
                using (var context = new DatabaseContext())
                {
                    context.TblUser.Add(UserInfo);
                    context.SaveChanges();
                }

                MessageBox.Show("User 정보가 추가했습니다.");
                SearchData();
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
                var dataRowView = selectedItem as DataRowView;

                if (dataRowView != null)
                {
                    UserInfo = new User()
                    {
                        Index       = (int)dataRowView.Row.ItemArray[0],
                        Name        = dataRowView.Row.ItemArray[1].ToString(),
                        Age         = (short)dataRowView.Row.ItemArray[2],
                        PhoneNumber = dataRowView.Row.ItemArray[3].ToString(),
                        IsInit      = (bool)dataRowView.Row.ItemArray[4],
                    };

                    Name        = UserInfo.Name;
                    Age         = UserInfo.Age.ToString();
                    PhoneNumber = UserInfo.PhoneNumber;


                    UserDataViewVisbility = Visibility.Visible;
                    UpdateVisibility      = Visibility.Visible;
                    InsertVisibility      = Visibility.Collapsed;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
