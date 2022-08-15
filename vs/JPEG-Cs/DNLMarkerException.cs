using System;
using System.Collections.Generic;
using System.Text;

namespace JPEG_Cs
{
    /// <summary>
    /// Класс исключения DNL маркера
    /// </summary>
    public class DNLMarkerException : Exception
    {
        /// <summary>
        /// Конструктор класса DNLMarkerException
        /// </summary>
        /// <param name="message"></param>
        public DNLMarkerException(string message) : base(message)
        { }
    }
}
