<Query Kind="Statements" />

using System.Text;
using System.Linq;

// Some OP2 units have every odd direction only use one animation frame.
// So for a unit with 8 directions and 4 frames, the frame count will look like this for all directions: 4,1,4,1,4,1,4,1
// In this case you will set below: numFrames = 4, numFacings = 8, numRepeats = 1.
// Sometimes units will use a single frame for several directions in a row, this frame count array will look like 4,1,1,1,4,1,1,1
// To build a sequence for this kind of arrangement, use these values:
// numFrames = 4, numFacings = 8, numRepeats = 3

const int startIndex = 10;    // The starting frame
const int numFrames = 4; 	  // How many frames in the animation
const int numFacings = 8;     // The number of directions it has
int numRepeats = 3;

// Code
var stringBuilder = new StringBuilder();
var numFramesTotal = numFrames * numFacings;
var tripletIndex = 0;
var outIndex = startIndex;
var resultList = new List<int>();
var repeatIndex = 0;
numRepeats -= 1;
for (var i = 0; i < numFramesTotal; i++)
{
	if (tripletIndex > numFrames)
		outIndex--;

	resultList.Add(outIndex);
	
	outIndex++;
	
	tripletIndex++;
	if (tripletIndex >= (2 * numFrames))
	{
		if (numRepeats > 0 && repeatIndex < numRepeats)
		{
			repeatIndex++;
			tripletIndex = numFrames;
		}
		else
		{
			if (numRepeats > 0)
			{
				repeatIndex = 0;
			}
		
			tripletIndex = 0;
		}
	}
}

string.Join(", ", resultList.ToArray()).Dump();