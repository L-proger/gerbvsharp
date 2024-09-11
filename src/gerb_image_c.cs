using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gerbvsharp.Gerbv;
using static System.Net.Mime.MediaTypeNames;


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
            image.netlist = new gerbv_net_t[1];

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
            image.netlist[0].layer = image.layers[0];
            image.netlist[0].state = image.states[0];

            if (string.IsNullOrWhiteSpace(type))
                image.info.type = "unknown";
            else
                image.info.type = type;

            /* the individual file parsers will have to set this. */
            image.info.attr_list = null;
            image.info.n_attr = 0;

            return image;
        }
    }
}