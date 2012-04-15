using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bs
{
	enum GenerationReason
	{
		NeverBuilt,
		Missing,
		InputFilesWereModifiedSinceLastGeneration,
		DifferentSettingsThanLastGeneration
	}
}
