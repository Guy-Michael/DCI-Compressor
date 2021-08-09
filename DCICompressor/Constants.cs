using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCICompressor
{
	struct Constants
	{
			public const ulong WHOLE = uint.MaxValue - 2;  //choose largest even number
			public const ulong HALF = WHOLE / 2;
			public const ulong QUARTER = HALF / 2;
			public const ulong PRECISION = 32;
	}
}
