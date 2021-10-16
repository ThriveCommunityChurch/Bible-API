using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleAPI.Core
{
    /// <summary>
    /// The book of the bible
    /// </summary>
    public class Book
    {
        /// <summary>
        /// The chronological book number
        /// </summary>
        public byte BookNumber { get; set; }

        /// <summary>
        /// The name of the book
        /// </summary>
        public string Name { get; set; }
    }
}