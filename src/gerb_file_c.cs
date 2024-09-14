using System.Text;
using System.Xml.Linq;
using static Gerbvsharp.Gerbv.gerbv_instruction_t;

namespace Gerbvsharp
{
    public static partial class Gerbv {
        public static partial gerb_file_t gerb_fopen(string filename){
            gerb_file_t result = new gerb_file_t();
            result.data = File.ReadAllText(filename).ToCharArray();
            result.datalen = result.data.LongLength;
            result.filename = filename;
            return result;
        }
        public static partial char gerb_fgetc(gerb_file_t fd){
            if(fd.ptr >= fd.datalen) {
                return char.MaxValue;
            }
            var result = fd.data[fd.ptr++];
            return result;
        }
        public static partial int gerb_fgetint(gerb_file_t fd, out int len) {
            var data = fd.data;
            int begin = fd.ptr;

            while (begin < data.Length && char.IsWhiteSpace((char)data[begin])) {
                begin++;
            }

            int end = begin;
            bool first = true;
            while ((end < data.Length) && (char.IsDigit((char)data[end]) || (data[end] == '+' && first) || (data[end] == '-' && first))) {
                end++;
                first = false;
            }

            int charCount = end - begin;
            if (charCount == 0) {
                throw new Exception("Failed to read integer from file");
            }

            char[] chars = new char[charCount];
            Array.Copy(data, begin, chars, 0, charCount);
            var intString = new string(chars);
            len = charCount;
            fd.ptr = end;
            return int.Parse(intString);
        }
        public static partial double gerb_fgetdouble(gerb_file_t fd){
            var data = fd.data;
            int begin = fd.ptr;

            while (begin < data.Length && char.IsWhiteSpace((char)data[begin])) {
                begin++;
            }

            int end = begin;
            bool first = true;
            while ((end < data.Length) && (char.IsDigit((char)data[end]) || (data[end] == '.') /*|| (data[end] == ',')*/ || (data[end] == '+' && first) || (data[end] == '-' && first))) {
                end++;
                first = false;
            }

            int charCount = end - begin;
            if (charCount == 0) {
                throw new Exception("Failed to read integer from file");
            }

            char[] chars = new char[charCount];
            Array.Copy(data, begin, chars, 0, charCount);
            var intString = new string(chars);
            fd.ptr = end;
            return double.Parse(intString);
        }
        public static partial string? gerb_fgetstring(gerb_file_t fd, char term){
            int strend = -1;
            for(int i = fd.ptr; i < fd.datalen; ++i) {
                if (fd.data[i] == term) {
                    strend = i; 
                    break;
                }
            }

            if(strend < 0) {
                return null;
            }

            var strlen = strend - fd.ptr;
            char[] chars = new char[strlen];
            Array.Copy(fd.data, fd.ptr, chars, 0, strlen);
            fd.ptr = (int)(fd.ptr + strlen);
            return new string(chars);
        }
        public static partial void gerb_ungetc(gerb_file_t fd){
            if(fd.ptr > 0) {
                fd.ptr--;
            }
        }
        public static partial void gerb_fclose(gerb_file_t fd){
            //Do nothing
        }

        /** Search for files in directories pointed out by paths, a NULL terminated
        * list of directories to search. If a string in paths starts with a $, then
        * characters to / (or string end if no /) is interpreted as a environment
        * variable.
        */
        public static partial string gerb_find_file(string filename, string[] paths){
            throw new NotImplementedException();
        }
    }
}