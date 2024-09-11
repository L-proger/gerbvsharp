using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerbvsharp {
    public static partial class Gerbv {
        /** Return string name of gerbv_aperture_type_t aperture type. */
        public static partial string gerbv_aperture_type_name(gerbv_aperture_type_t type) {
            /* These are the names of the valid apertures.  Please keep this in
             * sync with the gerbv_aperture_type_t enum defined in gerbv.h */
            string[] names = new[]{
                "none",          "circle", "rectangle", "oval", /* ovular (obround) aperture */
                "polygon",                                                  /* polygon aperture */
                "macro",                                                    /* RS274X macro */
                "circle macro",                                             /* RS274X circle macro */
                "outline macro",                                            /* RS274X outline macro */
                "polygon macro",                                            /* RS274X polygon macro */
                "moire macro",                                              /* RS274X moire macro */
                "thermal macro",                                            /* RS274X thermal macro */
                "line20 macro",                                             /* RS274X line (code 20) macro */
                "line21 macro",                                             /* RS274X line (code 21) macro */
                "line22 macro",                                             /* RS274X line (code 22) macro */
            };

            if ((int)type >= 0 && (int)type < names.Length)
                return names[(int)type];

            return "<undefined>";
        }

        /** Return string name of gerbv_interpolation_t interpolation. */
        public static partial string gerbv_interpolation_name(gerbv_interpolation_t interp) {
            /* These are the names of the interpolation method.  Please keep this
            * in sync with the gerbv_interpolation_t enum defined in gerbv.h */
            string[] names = new[]{
                "1X linear",    "10X linear",      "0.1X linear",    "0.01X linear", "CW circular",
                "CCW circular", "poly area start", "poly area stop", "deleted",
            };

            if ((int)interp >= 0 && (int)interp < names.Length)
                return names[(int)interp];

            return "<undefined>";
        }
    }
}
