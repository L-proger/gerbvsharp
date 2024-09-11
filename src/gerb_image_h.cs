using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerbvsharp {
    public static partial class Gerbv {
        /*
         * Check that the parsed gerber image is complete.
         * Returned errorcodes are:
         * 0: No problems
         * 1: Missing netlist
         * 2: Missing format
         * 4: Missing apertures
         * 8: Missing info
         * It could be any of above or'ed together
        */
        public enum gerb_verify_error_t {
            GERB_IMAGE_OK = 0,
            GERB_IMAGE_MISSING_NETLIST = 1,
            GERB_IMAGE_MISSING_FORMAT = 2,
            GERB_IMAGE_MISSING_APERTURES = 4,
            GERB_IMAGE_MISSING_INFO = 8,
        };

        //public static gerb_verify_error_t gerbv_image_verify(gerbv_image_t image);

        /* Dumps a written version of image to stdout */
        //public static void gerbv_image_dump(gerbv_image_t image);

        //public static gerbv_layer_t gerbv_image_return_new_layer(gerbv_layer_t previousLayer);

        //public static gerbv_netstate_t gerbv_image_return_new_netstate(gerbv_netstate_t previousState);

    }
}