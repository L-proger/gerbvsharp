namespace Gerbvsharp
{
    public static partial class Gerbv {

        public class gerb_state_t {
            public int                             curr_x;
            public int                             curr_y;
            public int                             prev_x;
            public int                             prev_y;
            public int                             delta_cp_x;
            public int                             delta_cp_y;
            public int                             curr_aperture;
            public int                             changed;
            public Gerbv.gerbv_aperture_state_t    aperture_state;
            public Gerbv.gerbv_interpolation_t     interpolation;
            public Gerbv.gerbv_interpolation_t     prev_interpolation;
            public Gerbv.gerbv_net_t?              parea_start_node;
            public Gerbv.gerbv_layer_t?            layer;
            public Gerbv.gerbv_netstate_t?         state;
            public int                             in_parea_fill;
            public int                             mq_on; /* Is multiquadrant circular iterpolation */
        };

        /*
        * parse gerber file pointed to by fd
        */
        public static partial gerbv_image_t parse_gerb(gerb_file_t fd, string directoryPath);
        //gboolean       gerber_is_rs274x_p(gerb_file_t* fd, gboolean* returnFoundBinary);
        //gboolean       gerber_is_rs274d_p(gerb_file_t* fd);
        //gerbv_net_t*   gerber_create_new_net(gerbv_net_t* currentNet, gerbv_layer_t* layer, gerbv_netstate_t* state);
//
        //gboolean gerber_create_new_aperture(
        //    gerbv_image_t* image, int* indexNumber, gerbv_aperture_type_t apertureType, gdouble parameter1, gdouble parameter2
        //);
//
        //void gerber_update_image_min_max(
        //    gerbv_render_size_t* boundingBox, double repeat_off_X, double repeat_off_Y, gerbv_image_t* image
        //);
        //void gerber_update_min_and_max(
        //    gerbv_render_size_t* boundingBox, gdouble x, gdouble y, gdouble apertureSizeX1, gdouble apertureSizeX2,
        //    gdouble apertureSizeY1, gdouble apertureSizeY2
        //);
        //const char* gerber_d_code_name(int d_code);
        //const char* gerber_g_code_name(int g_code);
        //const char* gerber_m_code_name(int m_code);
    }
}