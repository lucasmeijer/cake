using System;
using System.Collections.Generic;

namespace bs
{
	internal class BuildHistory
	{
		private readonly Dictionary<string, GenerationRecord> _records = new Dictionary<string, GenerationRecord>();

		public void AddRecord(GenerationRecord record)
		{
			_records[record.TargetFile] = record;
		}

		public GenerationRecord FindRecordFor(string targetFile)
		{
			return _records[targetFile];
		}
	}
}