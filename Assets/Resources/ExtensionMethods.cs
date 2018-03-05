using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static partial class ExtensionMethods
{
    public static int GetNextPositiontWrap<TElement>(this IEnumerable<TElement> enumerable, int currentPosition)
    {
        var hasAdditionalElements = currentPosition + 1 < enumerable.Count();

        var nextPosition = hasAdditionalElements
            ? currentPosition + 1
            : 0;

        return nextPosition;
    }

    public static int GetPreviousPositiontWrap<TElement>(this IEnumerable<TElement> enumerable, int currentPosition)
    {
        var hasAdditionalElements = currentPosition - 1 >= 0;

        var nextPosition = hasAdditionalElements
            ? currentPosition - 1
            : enumerable.Count() - 1;

        return nextPosition;
    }
}