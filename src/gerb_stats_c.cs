using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Gerbvsharp.Gerbv;
using static System.Net.Mime.MediaTypeNames;

namespace Gerbvsharp {
    public static partial class Gerbv {
        /* ------------------------------------------------------- */
        /** Allocates a new gerbv_stats structure
           @return gerbv_stats pointer on success, null on ERROR */
        public static partial gerbv_stats_t? gerbv_stats_new() {

            gerbv_stats_t stats;
            gerbv_error_list_t error_list;
            gerbv_aperture_list_t aperture_list;
            gerbv_aperture_list_t D_code_list;

            /* Malloc space for new stats struct.  Return null if error. */
            if (null == (stats = new gerbv_stats_t())) {
                return null;
            }

            /* Initialize error list */
            error_list = gerbv_stats_new_error_list();
            if (error_list == null) {
                throw new Exception("malloc error_list failed in %s()");
            }

            stats.error_list = error_list;

            /* Initialize aperture list */
            aperture_list = gerbv_stats_new_aperture_list();
            if (aperture_list == null) {
                throw new Exception("malloc aperture_list failed in %s()");
            }
            stats.aperture_list = aperture_list;

            /* Initialize D codes list */
            D_code_list = gerbv_stats_new_aperture_list();
            if (D_code_list == null) {
                throw new Exception("malloc D_code_list failed in %s()");
            }
            stats.D_code_list = D_code_list;

            return stats;
        }

        /* ------------------------------------------------------- */
        public static partial gerbv_error_list_t? gerbv_stats_new_error_list() {
            gerbv_error_list_t error_list;

            /* Malloc space for new error_list struct.  Return null if error. */
            if (null == (error_list = new gerbv_error_list_t())) {
                return null;
            }

            error_list.layer = -1;
            error_list.error_text = null;
            error_list.next = null;
            return error_list;
        }

        /* ------------------------------------------------------- */
        public static void gerbv_stats_printf(gerbv_error_list_t list, gerbv_message_type_t type, int layer, string text) {
            gerbv_stats_add_error(list, layer, text, type);
        }

        /** Escape special ASCII char ('\n', '\0'). Return C string with escaped
        * special char or original char in integer. Use gerbv_escape_char(char) macro
        * instead of this function.
        */
        /* ------------------------------------------------------- */
        public static partial int gerbv_escape_char_return_int(char c) {
            ushort ec = 0;

            ec = '\\';

            switch (c) {
                case '\0': ec |= ((ushort)'0') << 8; break;
                case '\a': ec |= ((ushort)'a') << 8; break;
                case '\b': ec |= ((ushort)'b') << 8; break;
                case '\f': ec |= ((ushort)'f') << 8; break;
                case '\n': ec |= ((ushort)'n') << 8; break;
                case '\r': ec |= ((ushort)'r') << 8; break;
                case '\t': ec |= ((ushort)'t') << 8; break;
                case '\v': ec |= ((ushort)'v') << 8; break;
                case '\\': ec |= ((ushort)'\\') << 8; break;
                case  '"': ec |= ((ushort)'"') << 8; break;
                default: ec |= (ushort)((ushort)c << 8); break;
            }
            return (char)ec;
        }

        /* ------------------------------------------------------- */
        public static void gerbv_stats_add_error(gerbv_error_list_t error_list_in, int layer, string error_text, gerbv_message_type_t type) {

            gerbv_error_list_t error_list_new;
            gerbv_error_list_t error_last = null;
            gerbv_error_list_t error;

            /* First handle case where this is the first list element */
            if (error_list_in.error_text == null) {
                error_list_in.layer = layer;
                error_list_in.error_text = error_text;
                error_list_in.type = type;
                error_list_in.next = null;
                return;
            }

            /* Next check to see if this error is already in the list */
            for (error = error_list_in; error != null; error = error.next) {
                if ((error.error_text == error_text) && (error.layer == layer)) {
                    return; /* This error text is already in the error list */
                }
                error_last = error; /* point to last element in error list */
            }
            /* This error text is unique.  Therefore, add it to the list */

            /* Now malloc space for new error list element */
            if (null == (error_list_new = new gerbv_error_list_t())) {
                GERB_FATAL_ERROR("malloc error_list failed in %s()");
            }

            /* Set member elements */
            error_list_new.layer = layer;
            error_list_new.error_text = error_text;
            error_list_new.type = type;
            error_list_new.next = null;
            error_last.next = error_list_new;

            return;
        }


        /* ------------------------------------------------------- */
        public static partial gerbv_aperture_list_t? gerbv_stats_new_aperture_list() {
            gerbv_aperture_list_t? aperture_list;
            int i;

            //Console.WriteLine("Mallocing new gerb aperture list");
            /* Malloc space for new aperture_list struct.  Return null if error. */
            if (null == (aperture_list = new gerbv_aperture_list_t())) {
                throw new Exception("malloc new gerb aperture list failed in %s()");
            }

            //Console.WriteLine("   Placing values in certain structs.");
            aperture_list.number = -1;
            aperture_list.count = 0;
            aperture_list.type = 0;
            for (i = 0; i < 5; i++) {
                aperture_list.parameter[i] = 0.0;
            }
            aperture_list.next = null;
            return aperture_list;
        }
    }
}
