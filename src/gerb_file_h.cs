namespace Gerbvsharp
{
    public static partial class Gerbv {
        public class gerb_file_t {
            public System.IO.FileStream fd;       /* File descriptor */
            public int   fileno;   /* The integer version of fd */
            public char[] data;     /* Pointer to data mmaped in. May not be changed, use ptr */
            public long   datalen;  /* Length of mmaped data ie file length */
            public int   ptr;      /* Index in data where we are reading */
            public string filename; /* File name */
        };

        public static partial gerb_file_t gerb_fopen(string filename);
        public static partial char          gerb_fgetc(gerb_file_t fd);
        public static partial int          gerb_fgetint(gerb_file_t fd, out int len); /* If len != NULL, returns number
                                        of chars parsed in len */
        public static partial double gerb_fgetdouble(gerb_file_t fd);
        public static partial string?  gerb_fgetstring(gerb_file_t fd, char term);
        public static partial void   gerb_ungetc(gerb_file_t fd);
        public static partial void   gerb_fclose(gerb_file_t fd);

        /** Search for files in directories pointed out by paths, a NULL terminated
        * list of directories to search. If a string in paths starts with a $, then
        * characters to / (or string end if no /) is interpreted as a environment
        * variable.
        */
        public static partial string gerb_find_file(string filename, string[] paths);
    }
}