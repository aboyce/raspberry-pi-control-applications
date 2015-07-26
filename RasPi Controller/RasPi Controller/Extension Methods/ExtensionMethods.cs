namespace RasPi_Controller.Extension_Methods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Removes extra characters that RichTextBoxes may add \r\n to the end of the string.
        /// </summary>
        /// <param name="stringToClean">The string to clean.</param>
        /// <returns>The string without the \r\n.</returns>
        public static string Clean(this string stringToClean)
        {
            return stringToClean.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }
    }
}
