using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace UserManagementSystem.Database.Entity
{
    public class User : IDataErrorInfo
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int    Index        { get; set; }
        
        [Column(Order = 1, TypeName = "NVARCHAR(5)")]
        [Required(ErrorMessage ="이름은 필수로 입력해야하는 항목입니다.")]
        [RegularExpression(@"^[가-힣]{1,5}$", ErrorMessage = "이름을 한글로 입력해주세요.")]
        public string Name         { get; set; }
        
        [Column(Order = 2)]
        [Required(ErrorMessage = "나이는 필수로 입력해야하는 항목입니다.")]
        [Range(0, 200, ErrorMessage = "나이는 0~200 사이에 값만 입력됩니다.")]
        public short  Age          { get; set; }
        
        [Column(Order = 3, TypeName = "NVARCHAR(11)")]
        [Required(ErrorMessage = "전화번호는 필수로 입력해야하는 항목입니다.")]
        [RegularExpression(@"01{1}[016789]{1}[0-9]{7,8}", ErrorMessage = "전화번호를 확인해주세요.(숫자만 입력해주세요)")]
        public string PhoneNumber  { get; set; }
        //초기 생성 여부를 확인하기 위한 컬럼
        
        [Column(Order = 4)]
        public bool IsInit         { get; set; } = false;

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;

                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        result = "이름은 필수로 입력해야하는 항목입니다.";
                    }
                    else if (!Regex.IsMatch(Name, @"^[가-힣]{1,5}$"))
                    {
                        result = "이름을 한글로 입력해주세요.";
                    }
                }
                else if (columnName == "Age")
                {
                    if (Age < 0 || Age > 200)
                    {
                        return "나이는 0~200 사이에 값만 입력됩니다.";
                    }
                }
                else if (columnName == "PhoneNumber")
                {
                    if (string.IsNullOrEmpty(PhoneNumber))
                    {
                        result = "전화번호는 필수로 입력해야하는 항목입니다.";
                    }
                    else if (!Regex.IsMatch(PhoneNumber, @"01{1}[016789]{1}[0-9]{7,8}"))
                    {
                        result = "전화번호를 확인해주세요.(숫자만 입력해주세요)";
                    }
                }

                return result;
            } 
        }

        public string Error
    {
        get
        {
            return "";
        }
    }

    }
}
