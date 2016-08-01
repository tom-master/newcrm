using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Dto.Dto
{
    public class TodayRecommendAppDto : BaseDto
    {
        public String Name { get; set; }

        public Int32 UserCount { get; set; }

        public String AppIcon { get; set; }

        public Int32 StartCount { get; set; }

        public Boolean IsInstall { get; set; }

        public String Remark { get; set; }

        public String Style { get; set; }
    }
}
