using System;

namespace NewCRM.Dto.Dto
{
    public sealed class TodayRecommendAppDto : BaseDto
    {
        public String Name { get; set; }

        public Int32 UseCount { get; set; }

        public String AppIcon { get; set; }

        public Double StartCount { get; set; }

        public Boolean IsInstall { get; set; }

        public String Remark { get; set; }

        public String Style { get; set; }
    }
}
