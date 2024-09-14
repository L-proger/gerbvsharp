using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerbvsharp {
    public static partial class libc {

        [ThreadStatic]
        private static int _strtok_position;
        [ThreadStatic]
        private static string _strtok_data;

        public static string? strtok(string tokenString, char[] delimiters) {
            if (tokenString != null) {
                _strtok_position = 0;
                _strtok_data = tokenString;
            }

            if (_strtok_data == null) {
                return null;
            }

            //skip leading delimiters
            while ((_strtok_position < _strtok_data.Length) && delimiters.Any(x => x == _strtok_data[_strtok_position])) {
                _strtok_position++;
            }

            int tokenBegin = _strtok_position;

            //find next delimiter
            while ((_strtok_position < _strtok_data.Length) && !delimiters.Any(x => x == _strtok_data[_strtok_position])) {
                _strtok_position++;
            }

            int tokenLength = _strtok_position - tokenBegin;
            if (tokenLength == 0) {
                return null;
            } else {
                string token = _strtok_data.Substring(tokenBegin, tokenLength);

                if (_strtok_position == _strtok_data.Length) {
                    _strtok_data = null;
                    _strtok_position = 0;
                }
                return token;
            }
        }

        public static string? strtok(string tokenString, string delimiters) {
            return strtok(tokenString, delimiters.ToCharArray());
        }
    }
}
