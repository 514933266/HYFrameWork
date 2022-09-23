using System.ComponentModel.DataAnnotations;

namespace HYFrameWork.Demo.Excel
{
    public class ExcelImportModel
    {

        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        [Display(Name = "手机号")]
        public string Mobile { get; set; }

        [Display(Name = "年龄")]
        public int Age { get; set; }
    }
}
