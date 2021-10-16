using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleAPI.Core
{
    /// <summary>
    /// Bible passage
    /// </summary>
    public class Passage
    {
        /// <summary>
        /// The Id of the book that this passage resides in
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        /// The chronological verse number
        /// </summary>
        public int VerseNumber { get; set; }

        /// <summary>
        /// The language that this verse is translated to
        /// </summary>
        public LanguageCode Language { get; set; } 

        /// <summary>
        /// The version that this passage is derived from
        /// </summary>
        public VersionCode Version { get; set; }

        /// <summary>
        /// The passage text
        /// </summary>
        public string Text { get; set; }
    }
}