using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.CommonConstants
{
	public static class ConvertToTiengVietKhongDauConstants
	{
		#region Vinh - Xử lý tiếng việt không dấu cho mã QR
		private static readonly string[] VietnameseSigns = new string[]
		{
		"aAeEoOuUiIdDyY",
		"áàạảãâấầậẩẫăắằặẳẵ",
		"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
		"éèẹẻẽêếềệểễ",
		"ÉÈẸẺẼÊẾỀỆỂỄ",
		"óòọỏõôốồộổỗơớờợởỡ",
		"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
		"úùụủũưứừựửữ",
		"ÚÙỤỦŨƯỨỪỰỬỮ",
		"íìịỉĩ",
		"ÍÌỊỈĨ",
		"đ",
		"Đ",
		"ýỳỵỷỹ",
		"ÝỲỴỶỸ"
		};
		/// <summary>
		/// Ham chuyen tieng viet co dau thanh khong dau
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RemoveSign4VietnameseString(string str)

		{
			//Tiến hành thay thế , lọc bỏ dấu cho chuỗi
			for (int i = 1; i < VietnameseSigns.Length; i++)
			{
				for (int j = 0; j < VietnameseSigns[i].Length; j++)
					str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
			}
			return str;
		}
		#endregion
	}
}
