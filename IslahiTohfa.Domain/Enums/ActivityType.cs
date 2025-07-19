using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Domain.Enums
{
    public enum ActivityType
    {
        BookViewed = 0,
        BookDownloaded = 1,
        BookLiked = 2,
        BookUnliked = 3,
        BookRated = 4,
        BookCommented = 5,
        BookShared = 6,
        BookBookmarked = 7,
        ReadingStarted = 8,
        ReadingCompleted = 9,
        ReadingProgress = 10,
        BookSearched = 11
    }
}
