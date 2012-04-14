using System;
using System.Collections.Generic;

namespace bs
{
	public class BuildHistory
	{
		private readonly Dictionary<string, GenerationRecord> _records = new Dictionary<string, GenerationRecord>();

		public void AddRecord(GenerationRecord record)
		{
			_records[record.TargetFile] = record;
		}

		public GenerationRecord FindRecordFor(string targetFile)
		{
			GenerationRecord result;
			_records.TryGetValue(targetFile,out result);
			return result;
		}
	}
}