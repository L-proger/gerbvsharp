using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerbvsharp {
    public static partial class Gerbv {

        /* ===================  Prototypes ================ */
        public static partial gerbv_error_list_t? gerbv_stats_new_error_list();
        //void gerbv_stats_printf(gerbv_error_list_t* list, gerbv_message_type_t type, int layer, const char* text, ...);
        //void gerbv_stats_add_error(gerbv_error_list_t* error_list_in, int layer, const char* error_text, gerbv_message_type_t type);
        public static int gerbv_escape_char(char c) { return gerbv_escape_char_return_int(c); }
        public static partial int gerbv_escape_char_return_int(char c);

        public static partial gerbv_aperture_list_t? gerbv_stats_new_aperture_list();
        //void gerbv_stats_add_aperture( gerbv_aperture_list_t* aperture_list_in, int layer, int number, gerbv_aperture_type_t type, double parameter[5] );
        //void gerbv_stats_add_to_D_list(gerbv_aperture_list_t* D_list_in, int number);
        //int gerbv_stats_increment_D_list_count(gerbv_aperture_list_t* D_list_in, int number, int count, gerbv_error_list_t* error);


    }
}
