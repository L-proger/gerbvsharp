using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Gerbvsharp.Gerbv;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Gerbvsharp {
    public static partial class Gerbv {
        public struct gerb_translation_entry_t {
            int oldAperture;
            int newAperture;
        };

        public static gerbv_image_t gerbv_create_image(gerbv_image_t image, string type) {
            //gerbv_destroy_image(image);

            /* Malloc space for image */
            image = new gerbv_image_t();

            /* Malloc space for image->netlist */
            image.netlist = new gerbv_net_t();

            /* Malloc space for image->info */
            image.info = new gerbv_image_info_t();

            /* Set aside position for stats struct */
            image.gerbv_stats = null;
            image.drill_stats = null;

            image.info.min_x = double.PositiveInfinity;
            image.info.min_y = double.PositiveInfinity;
            image.info.max_x = -double.NegativeInfinity;
            image.info.max_y = -double.NegativeInfinity;

            /* create our first layer and fill with non-zero default values */
            image.layers = new gerbv_layer_t[1];
            image.layers[0].stepAndRepeat.X = 1;
            image.layers[0].stepAndRepeat.Y = 1;
            image.layers[0].polarity = gerbv_polarity_t.GERBV_POLARITY_DARK;

            /* create our first netstate and fill with non-zero default values */
            image.states = new gerbv_netstate_t[1];
            image.states[0].scaleA = 1;
            image.states[0].scaleB = 1;

            /* fill in some values for our first net */
            image.netlist.layer = image.layers[0];
            image.netlist.state = image.states[0];

            if (string.IsNullOrWhiteSpace(type))
                image.info.type = "unknown";
            else
                image.info.type = type;

            /* the individual file parsers will have to set this. */
            image.info.attr_list = null;
            image.info.n_attr = 0;

            return image;
        }

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
        public static partial gerb_verify_error_t gerbv_image_verify(gerbv_image_t image) {
            gerb_verify_error_t error = gerb_verify_error_t.GERB_IMAGE_OK;
            int i, n_nets;
            
            gerbv_net_t net;

            if (image.netlist == null)
                error |= gerb_verify_error_t.GERB_IMAGE_MISSING_NETLIST;
            if (image.format == null)
                error |= gerb_verify_error_t.GERB_IMAGE_MISSING_FORMAT;
            if (image.info == null)
                error |= gerb_verify_error_t.GERB_IMAGE_MISSING_INFO;

            /* Count how many nets we have */
            n_nets = 0;
            if (image.netlist != null) {
                for (net = image.netlist.next; net != null; net = net.next) {
                    n_nets++;
                }
            }

            /* If we have nets but no apertures are defined, then complain */
            if (n_nets > 0) {
                for (i = 0; i < APERTURE_MAX && image.aperture[i] == null; i++)
                    ;
                if (i == APERTURE_MAX)
                    error |= gerb_verify_error_t.GERB_IMAGE_MISSING_APERTURES;
            }

            return error;
        } /* gerb_image_verify */

        public static void gerbv_image_aperture_state(gerbv_aperture_state_t state) {
            switch (state) {
                case gerbv_aperture_state_t.GERBV_APERTURE_STATE_OFF: Console.Write("..state off"); break;
                case gerbv_aperture_state_t.GERBV_APERTURE_STATE_ON: Console.Write("..state on"); break;
                case gerbv_aperture_state_t.GERBV_APERTURE_STATE_FLASH: Console.Write("..state flash"); break;
                default: Console.Write("..state unknown"); break;
            }
        }

        /* Dumps a written version of image to stdout */
        public static partial void gerbv_image_dump(gerbv_image_t image) {
            int i, j;
            gerbv_aperture_t[] aperture;
            gerbv_net_t? net;

            /* Apertures */
            Console.WriteLine("Apertures:");
            aperture = image.aperture;
            for (i = 0; i< image.aperture.Length; i++) {
                if (aperture[i] != null) {
                    Console.Write($" Aperture no:{i} is an ");
                    switch (aperture[i].type) {
                        case gerbv_aperture_type_t.GERBV_APTYPE_CIRCLE: Console.Write("circle"); break;
                        case gerbv_aperture_type_t.GERBV_APTYPE_RECTANGLE: Console.Write("rectangle"); break;
                        case gerbv_aperture_type_t.GERBV_APTYPE_OVAL: Console.Write("oval"); break;
                        case gerbv_aperture_type_t.GERBV_APTYPE_POLYGON: Console.Write("polygon"); break;
                        case gerbv_aperture_type_t.GERBV_APTYPE_MACRO: Console.Write("macro"); break;
                        default: Console.Write("unknown"); break;
                    }
                    for (j = 0; j<aperture[i].nuf_parameters; j++) {
                        Console.Write($" {aperture[i].parameter[j]}");
                    }
                    Console.WriteLine();
                }
            }

            /* Netlist */
            net = image.netlist;
            while (net != null) {
                Console.Write($"({net.start_x},{net.start_y})->({net.stop_x},{net.stop_y}) with {net.aperture} (");
                Console.Write($"{gerbv_interpolation_name(net.interpolation)}");
                gerbv_image_aperture_state(net.aperture_state);
                Console.WriteLine(")");
                net = net.next;
            }
        } /* gerbv_image_dump */

        public static gerbv_layer_t gerbv_image_return_new_layer(gerbv_layer_t previousLayer) {
            gerbv_layer_t newLayer = new gerbv_layer_t();
            previousLayer.next = newLayer;
            /* clear this boolean so we only draw the knockout once */
            newLayer.knockout = previousLayer.knockout;
            newLayer.knockout.firstInstance = false;
            newLayer.name = null;
            newLayer.next = null;
            newLayer.polarity = previousLayer.polarity;
            newLayer.rotation = previousLayer.rotation;
            newLayer.stepAndRepeat = previousLayer.stepAndRepeat;
            return newLayer;
        } /* gerbv_image_return_new_layer */

        public static gerbv_netstate_t gerbv_image_return_new_netstate(gerbv_netstate_t previousState) {
            gerbv_netstate_t newState = new gerbv_netstate_t();
            previousState.next = newState;

            newState.axisSelect = previousState.axisSelect;
            newState.mirrorState = previousState.mirrorState;
            newState.unit = previousState.unit;
            newState.offsetA = previousState.offsetA;
            newState.offsetB = previousState.offsetB;
            newState.scaleA = 1.0;
            newState.scaleB = 1.0;
            return newState;
        } /* gerbv_image_return_new_netstate */

    }
}