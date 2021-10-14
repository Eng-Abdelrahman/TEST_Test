using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Helpers
{
     public class PreviewStatus
    {
        public static string IsCancelled = "الغيت";
        public static string IsRejected = "تم رفضها";
        public static string IsSucceded = "تم قبولها";
        public static string IsSemiSuccess = "تم قبول بعض عناصرها";
        public static string IsSuccessAndSuspend = "تم قبول بعض عناصرهاوهناك عناصر معلقة";
        public static string IsPostponed = "تم تاجيل الميعاد";
        public static string IsSuspended ="يوجد عناصر معلقة";
        public static string IsUnsuccessefull = "تمت ولم يحب العميل اي اختيار";
        public static string IsUndefined = "غير معروف حالتها لعدم تحديد حاله عناصرها بعد!";


    }
}
