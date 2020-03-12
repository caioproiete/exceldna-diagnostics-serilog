using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog.Events;
using Xunit;

namespace ExcelDna.Diagnostics.Serilog.Tests
{
    public class LogEventLevelConvertTests
    {
        [Theory]
        [InlineData(TraceEventType.Critical, LogEventLevel.Fatal)]
        [InlineData(TraceEventType.Error, LogEventLevel.Error)]
        [InlineData(TraceEventType.Warning, LogEventLevel.Warning)]
        [InlineData(TraceEventType.Information, LogEventLevel.Information)]
        [InlineData(TraceEventType.Start, LogEventLevel.Debug)]
        [InlineData(TraceEventType.Stop, LogEventLevel.Debug)]
        [InlineData(TraceEventType.Suspend, LogEventLevel.Debug)]
        [InlineData(TraceEventType.Resume, LogEventLevel.Debug)]
        [InlineData(TraceEventType.Transfer, LogEventLevel.Debug)]
        [InlineData(TraceEventType.Verbose, LogEventLevel.Verbose)]
        public void TraceEventType_is_converted_to_the_right_LogEventLevel(TraceEventType traceEventType, LogEventLevel expectedLogEventLevel)
        {
            var result = LogEventLevelConvert.ToLogEventLevel(traceEventType);
            Assert.Equal(expectedLogEventLevel, result);
        }

        [Fact]
        public void Throws_ArgumentOutOfRangeException_for_invalid_TraceEventType()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => LogEventLevelConvert.ToLogEventLevel(0));
        }

        public static readonly IEnumerable<object[]> AllTraceEventTypes = Enum.GetValues(typeof(TraceEventType))
            .Cast<object>().Select(v => new[] { v });

        public static readonly IEnumerable<LogEventLevel> AllLogEventLevels = Enum.GetValues(typeof(LogEventLevel))
            .Cast<LogEventLevel>();

        [Theory]
        [MemberData(nameof(AllTraceEventTypes))]
        public void Can_convert_any_TraceEventType_to_a_LogEventLevel(TraceEventType traceEventType)
        {
            var result = LogEventLevelConvert.ToLogEventLevel(traceEventType);
            Assert.Contains(result, AllLogEventLevels);
        }
    }
}