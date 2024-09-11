using System.Reflection.Metadata;

namespace Gerbvsharp
{
    public static partial class Gerbv {
        public const int APERTURE_MIN = 10;
        public const int APERTURE_MAX = 9999;

        /*
        * Maximum number of aperture parameters is set by the outline aperture macro.
        * There (p. 62) is defined up to 5000 points in outline. So 5000 points with x
        * and y plus outline shape code, exposure, # of vertices, and duplication of
        * start/end point gives
        */
        public const int APERTURE_PARAMETERS_MAX = 10006;
        public const int GERBV_SCALE_MIN         = 10;
        public const int GERBV_SCALE_MAX         = 40000;
        public const int MAX_ERRMSGLEN           = 25;
        public const int MAX_COORDLEN            = 28;
        public const int MAX_DISTLEN             = 180;
        public const int MAX_STATUSMSGLEN        = (MAX_ERRMSGLEN + MAX_COORDLEN + MAX_DISTLEN);

        /*
        * Files only have a limited precision in their data, so when interpreting
        * layer rotations or linear size that have been read from a project file, we
        * have to tolerate a certain amount of error.
        */
        public const double GERBV_PRECISION_ANGLE_RAD   = 1e-6;
        public const double GERBV_PRECISION_LINEAR_INCH = 1e-6;


        /* Convert between unscaled gerber coordinates and other units */
        /* XXX NOTE: Currently unscaled units are assumed as inch, this is not
        XXX necessarily true for all files */
        public static double COORD2INS(double c){ return c; }
        public static double COORD2MILS(double c){ return c * 1000.0; }
        public static double COORD2MMS(double c){ return c * 25.4; }


        public static double DEG2RAD(double d){ return d * Math.PI  / 180.0; }
        public static double RAD2DEG(double r){ return r * 180.0  / Math.PI; }
  

        public static void GERB_FATAL_ERROR(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_ERROR, __VA_ARGS__)*/}
        public static void GERB_COMPILE_ERROR(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_CRITICAL, __VA_ARGS__)*/}
        public static void GERB_COMPILE_WARNING(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_WARNING, __VA_ARGS__)*/}
        public static void GERB_MESSAGE(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_MESSAGE, __VA_ARGS__)*/}

        /*! The aperture macro commands */
        public enum gerbv_opcodes_t {
            GERBV_OPCODE_NOP,   /*!< no operation */
            GERBV_OPCODE_PUSH,  /*!< push the instruction onto the stack */
            GERBV_OPCODE_PPUSH, /*!< push parameter onto stack */
            GERBV_OPCODE_PPOP,  /*!< pop parameter from stack */
            GERBV_OPCODE_ADD,   /*!< mathmatical add operation */
            GERBV_OPCODE_SUB,   /*!< mathmatical subtract operation */
            GERBV_OPCODE_MUL,   /*!< mathmatical multiply operation */
            GERBV_OPCODE_DIV,   /*!< mathmatical divide operation */
            GERBV_OPCODE_PRIM   /*!< draw macro primative */
        };

        /*! The different message types used in libgerbv */
        public enum gerbv_message_type_t {
            GERBV_MESSAGE_FATAL,   /*!< processing cannot continue */
            GERBV_MESSAGE_ERROR,   /*!< something went wrong, but processing can still continue */
            GERBV_MESSAGE_WARNING, /*!< something was encountered that may provide the wrong output */
            GERBV_MESSAGE_NOTE     /*!< an irregularity was encountered, but needs no intervention */
        };

        /*! The different aperture types available.
        *  Please keep these in sync with the aperture names defined in
        *  gerbv_aperture_type_name() in gerbv.c */
        public enum gerbv_aperture_type_t {
            GERBV_APTYPE_NONE,          /*!< no aperture used */
            GERBV_APTYPE_CIRCLE,        /*!< a round aperture */
            GERBV_APTYPE_RECTANGLE,     /*!< a rectangular aperture */
            GERBV_APTYPE_OVAL,          /*!< an ovular (obround) aperture */
            GERBV_APTYPE_POLYGON,       /*!< a polygon aperture */
            GERBV_APTYPE_MACRO,         /*!< a RS274X macro */
            GERBV_APTYPE_MACRO_CIRCLE,  /*!< a RS274X circle macro */
            GERBV_APTYPE_MACRO_OUTLINE, /*!< a RS274X outline macro */
            GERBV_APTYPE_MACRO_POLYGON, /*!< a RS274X polygon macro */
            GERBV_APTYPE_MACRO_MOIRE,   /*!< a RS274X moire macro */
            GERBV_APTYPE_MACRO_THERMAL, /*!< a RS274X thermal macro */
            GERBV_APTYPE_MACRO_LINE20,  /*!< a RS274X vector line (code 20) macro */
            GERBV_APTYPE_MACRO_LINE21,  /*!< a RS274X centered line (code 21) macro */
            GERBV_APTYPE_MACRO_LINE22   /*!< a RS274X lower left line (code 22) macro */
        };

        /*! The current state of the aperture drawing tool */
        public enum gerbv_aperture_state_t {
            GERBV_APERTURE_STATE_OFF,  /*!< tool drawing is off, and nothing will be drawn */
            GERBV_APERTURE_STATE_ON,   /*!< tool drawing is on, and something will be drawn */
            GERBV_APERTURE_STATE_FLASH /*!< tool is flashing, and will draw a single aperture */
        };

        /*! The circle aperture macro parameter indexes */
        public enum gerbv_aptype_macro_circle_index_t {
            CIRCLE_EXPOSURE,
            CIRCLE_DIAMETER,
            CIRCLE_CENTER_X,
            CIRCLE_CENTER_Y,
        };

        public enum gerbv_aptype_macro_outline_index_t {
            OUTLINE_EXPOSURE,
            OUTLINE_NUMBER_OF_POINTS,
            OUTLINE_FIRST_X, /* x0 */
            OUTLINE_FIRST_Y, /* y0 */
            /* x1, y1, x2, y2, ..., rotation */
            OUTLINE_ROTATION, /* Rotation index is correct if outline has
                        no point except first */
        };

        /* Point number is from 0 (first) to (including) OUTLINE_NUMBER_OF_POINTS */
        public static int OUTLINE_X_IDX_OF_POINT(int number){ return 2 * number + (int)gerbv_aptype_macro_outline_index_t.OUTLINE_FIRST_X; }
        public static int OUTLINE_Y_IDX_OF_POINT(int number){ return 2 * number + (int)gerbv_aptype_macro_outline_index_t.OUTLINE_FIRST_Y; }
        public static int OUTLINE_ROTATION_IDX(int[] param_array){ return (int)param_array[(int)gerbv_aptype_macro_outline_index_t.OUTLINE_NUMBER_OF_POINTS] * 2 + (int)gerbv_aptype_macro_outline_index_t.OUTLINE_ROTATION; }

        public enum gerbv_aptype_macro_polygon_index_t {
            POLYGON_EXPOSURE,
            POLYGON_NUMBER_OF_POINTS,
            POLYGON_CENTER_X,
            POLYGON_CENTER_Y,
            POLYGON_DIAMETER,
            POLYGON_ROTATION,
        };

        public enum gerbv_aptype_macro_moire_index_t {
            MOIRE_CENTER_X,
            MOIRE_CENTER_Y,
            MOIRE_OUTSIDE_DIAMETER,
            MOIRE_CIRCLE_THICKNESS,
            MOIRE_GAP_WIDTH,
            MOIRE_NUMBER_OF_CIRCLES,
            MOIRE_CROSSHAIR_THICKNESS,
            MOIRE_CROSSHAIR_LENGTH,
            MOIRE_ROTATION,
        };

        public enum gerbv_aptype_macro_thermal_index_t {
            THERMAL_CENTER_X,
            THERMAL_CENTER_Y,
            THERMAL_OUTSIDE_DIAMETER,
            THERMAL_INSIDE_DIAMETER,
            THERMAL_CROSSHAIR_THICKNESS,
            THERMAL_ROTATION,
        };

        /*! The vector line aperture macro parameter indexes */
        public enum gerbv_aptype_macro_line20_index_t {
            LINE20_EXPOSURE,
            LINE20_LINE_WIDTH,
            LINE20_WIDTH = LINE20_LINE_WIDTH, /* Unification alias */
            LINE20_START_X,
            LINE20_START_Y,
            LINE20_END_X,
            LINE20_END_Y,
            LINE20_ROTATION,
        };

        /*! The centered line aperture macro parameter indexes */
        public enum gerbv_aptype_macro_line21_index_t {
            LINE21_EXPOSURE,
            LINE21_WIDTH,
            LINE21_HEIGHT,
            LINE21_CENTER_X,
            LINE21_CENTER_Y,
            LINE21_ROTATION,
        };

        /*! The lower left line aperture macro parameter indexes */
        public enum gerbv_aptype_macro_line22_index_t {
            LINE22_EXPOSURE,
            LINE22_WIDTH,
            LINE22_HEIGHT,
            LINE22_LOWER_LEFT_X,
            LINE22_LOWER_LEFT_Y,
            LINE22_ROTATION,
        };

        /*! The current unit used */
        public enum gerbv_unit_t {
            GERBV_UNIT_INCH,       /*!< inches */
            GERBV_UNIT_MM,         /*!< mm */
            GERBV_UNIT_UNSPECIFIED /*!< use default units */
        };

        /*! The different drawing polarities available */
        public enum gerbv_polarity_t {
            GERBV_POLARITY_POSITIVE, /*!< draw "positive", using the current layer's polarity */
            GERBV_POLARITY_NEGATIVE, /*!< draw "negative", reversing the current layer's polarity */
            GERBV_POLARITY_DARK,     /*!< add to the current rendering */
            GERBV_POLARITY_CLEAR     /*!< subtract from the current rendering */
        };

        /*! The decimal point parsing style used */
        public enum gerbv_omit_zeros_t {
            GERBV_OMIT_ZEROS_LEADING,    /*!< omit extra zeros before the decimal point */
            GERBV_OMIT_ZEROS_TRAILING,   /*!< omit extra zeros after the decimal point */
            GERBV_OMIT_ZEROS_EXPLICIT,   /*!< explicitly specify how many decimal places are used */
            GERBV_OMIT_ZEROS_UNSPECIFIED /*!< use the default parsing style */
        };

        /*! The coordinate system used */
        public enum gerbv_coordinate_t {
            GERBV_COORDINATE_ABSOLUTE,   /*!< all coordinates are absolute from a common origin */
            GERBV_COORDINATE_INCREMENTAL /*!< all coordinates are relative to the previous coordinate */
        };

        /*! The interpolation methods available.
        *  Please keep these in sync with the interpolation names defined in
        *  gerbv_interpolation_name() in gerbv.c */
        public enum gerbv_interpolation_t {
            GERBV_INTERPOLATION_LINEARx1,     /*!< draw a line */
            GERBV_INTERPOLATION_LINEARx10,    /*!< draw a line */
            GERBV_INTERPOLATION_LINEARx01,    /*!< draw a line */
            GERBV_INTERPOLATION_LINEARx001,   /*!< draw a line */
            GERBV_INTERPOLATION_CW_CIRCULAR,  /*!< draw an arc in the clockwise direction */
            GERBV_INTERPOLATION_CCW_CIRCULAR, /*!< draw an arc in the counter-clockwise direction */
            GERBV_INTERPOLATION_PAREA_START,  /*!< start a polygon draw */
            GERBV_INTERPOLATION_PAREA_END,    /*!< end a polygon draw */
            GERBV_INTERPOLATION_DELETED,       /*!< the net has been deleted by the user, and will not be drawn */
            GERBV_INTERPOLATION_x10 = GERBV_INTERPOLATION_LINEARx10 /* For backward compatibility */
        };

        public enum gerbv_encoding_t {
            GERBV_ENCODING_NONE,
            GERBV_ENCODING_ASCII,
            GERBV_ENCODING_EBCDIC,
            GERBV_ENCODING_BCD,
            GERBV_ENCODING_ISO_ASCII,
            GERBV_ENCODING_EIA
        };

        /*! The different layer types used */
        public enum gerbv_layertype_t {
            GERBV_LAYERTYPE_RS274X,           /*!< the file is a RS274X file */
            GERBV_LAYERTYPE_DRILL,            /*!< the file is an Excellon drill file */
            GERBV_LAYERTYPE_PICKANDPLACE_TOP, /*!< the file is a CSV pick and place file, top side */
            GERBV_LAYERTYPE_PICKANDPLACE_BOT, /*!< the file is a CSV pick and place file, bottom side */
        };

        public enum gerbv_knockout_type_t {
            GERBV_KNOCKOUT_TYPE_NOKNOCKOUT,
            GERBV_KNOCKOUT_TYPE_FIXEDKNOCK,
            GERBV_KNOCKOUT_TYPE_BORDER
        };

        public enum gerbv_mirror_state_t {
            GERBV_MIRROR_STATE_NOMIRROR,
            GERBV_MIRROR_STATE_FLIPA,
            GERBV_MIRROR_STATE_FLIPB,
            GERBV_MIRROR_STATE_FLIPAB
        };

        public enum gerbv_axis_select_t {
            GERBV_AXIS_SELECT_NOSELECT,
            GERBV_AXIS_SELECT_SWAPAB
        };

        public enum gerbv_image_justify_type_t {
            GERBV_JUSTIFY_NOJUSTIFY,
            GERBV_JUSTIFY_LOWERLEFT,
            GERBV_JUSTIFY_CENTERJUSTIFY
        };

        public class gerbv_HID_Attr_Val {
            public int    int_value;
            public string str_value;  // NOTE: memory here is released via `free()` (thus cannot be `const char*`)
            public double real_value;
        };

        public class gerbv_HID_Attribute {
            public string name;       // Allow 'const' variables (e.g., stored in read-only pages)
            public string help_text;  // Allow 'const' variables (e.g., stored in read-only pages)

            public enum type_{
                HID_Label,
                HID_Integer,
                HID_Real,
                HID_String,
                HID_Boolean,
                HID_Enum,
                HID_Mixed,
                HID_Path
            } ;

            public int                min_val, max_val; /* for integer and real */
            public gerbv_HID_Attr_Val default_val;      /* Also actual value for global attributes.  */
            public string[]       enumerations;
            /* If set, this is used for global attributes (i.e. those set
            statically with REGISTER_ATTRIBUTES below) instead of changing
            the default_val.  Note that a HID_Mixed attribute must specify a
            pointer to gerbv_HID_Attr_Val here, and HID_Boolean assumes this is
            "char *" so the value should be initialized to zero, and may be
            set to non-zero (not always one).  */
            public IntPtr value;
            public int   hash; /* for detecting changes. */
        };

        /*! A linked list of errors found in the files */
        public class gerbv_error_list_t {
            public int                  layer;
            public string?              error_text;
            public gerbv_message_type_t type;
            public gerbv_error_list_t?  next;
        };

        public class gerbv_instruction_t {
            public gerbv_opcodes_t opcode;

            public struct Data {
                public int   ival;
                public float fval;
            };
            public Data data;
            public gerbv_instruction_t? next;
        };

        public class gerbv_amacro_t {
            public string               name;
            public gerbv_instruction_t  program;
            public uint                 nuf_push; /* Nuf pushes in program to estimate stack size */
            public gerbv_amacro_t?       next;
        };

        public class gerbv_simplified_amacro_t {
            public gerbv_aperture_type_t           type;
            public double[]                          parameter;/*APERTURE_PARAMETERS_MAX*/
            public gerbv_simplified_amacro_t? next;
        };

        public class gerbv_aperture_t {
            public gerbv_aperture_type_t      type;
            public gerbv_amacro_t?            amacro;
            public gerbv_simplified_amacro_t? simplified;
            public double[]                     parameter; /*APERTURE_PARAMETERS_MAX*/
            public int                        nuf_parameters;
            public gerbv_unit_t               unit;
        };

        /* the gerb_aperture_list is used to keep track of
        * apertures used in stats reporting */
        public class gerbv_aperture_list_t {
            public int                         number;
            public int                         layer;
            public int                         count;
            public gerbv_aperture_type_t       type;
            public double[]                      parameter; /*[5]*/
            public gerbv_aperture_list_t? next;
        };

        /*! Contains statistics on the various codes used in a RS274X file */
        public class gerbv_stats_t{
            public gerbv_error_list_t?    error_list;
            public gerbv_aperture_list_t? aperture_list;
            public gerbv_aperture_list_t? D_code_list;
            public int layer_count;
            public int G0;
            public int G1;
            public int G2;
            public int G3;
            public int G4;
            public int G10;
            public int G11;
            public int G12;
            public int G36;
            public int G37;
            public int G54;
            public int G55;
            public int G70;
            public int G71;
            public int G74;
            public int G75;
            public int G90;
            public int G91;
            public int G_unknown;

            public int D1;
            public int D2;
            public int D3;
            /*    GHashTable *D_user_defined; */
            public int D_unknown;
            public int D_error;

            public int M0;
            public int M1;
            public int M2;
            public int M_unknown;

            public int X;
            public int Y;
            public int I;
            public int J;

            /* Must include % RS-274 codes */
            public int star;
            public int unknown;

        };

        /*! Linked list of drills found in active layers.  Used in reporting statistics */
        public class gerbv_drill_list_t {
            public int                drill_num;
            public double             drill_size;
            public string             drill_unit;
            public int                drill_count;
            public gerbv_drill_list_t? next;
        };

        /*! Struct holding statistics of drill commands used.  Used in reporting statistics */
        public class gerbv_drill_stats_t {
            public int layer_count;
 
            public gerbv_error_list_t? error_list;
            public gerbv_drill_list_t? drill_list;
            public int                 comment;
            public int                 F;
 
            public int G00;
            public int G01;
            public int G02;
            public int G03;
            public int G04;
            public int G05;
            public int G85;
            public int G90;
            public int G91;
            public int G93;
            public int G_unknown;
 
            public int M00;
            public int M01;
            public int M18;
            public int M25;
            public int M30;
            public int M31;
            public int M45;
            public int M47;
            public int M48;
            public int M71;
            public int M72;
            public int M95;
            public int M97;
            public int M98;
            public int M_unknown;
 
            public int R;
 
            public int unknown;
 
            /* used to total up the drill count across all layers/sizes */
            public int total_count;
 
            public string detect;

        };

        /*!  This defines a box location and size (used to rendering logic) */
        public class gerbv_render_size_t {
            public double left;   /*!< the X coordinate of the left side */
            public double right;  /*!< the X coordinate of the right side */
            public double bottom; /*!< the Y coordinate of the bottom side */
            public double top;    /*!< the Y coordinate of the top side */
        };

        public class gerbv_cirseg_t {
            public double cp_x;   /* center point x */
            public double cp_y;   /* center point y */
            public double width;  /* used as diameter */
            public double height; /* */
            public double angle1; /* in degrees */
            public double angle2; /* in degrees */
        };

        public class gerbv_step_and_repeat_t { /* SR parameters */
            public int    X;
            public int    Y;
            public double dist_X;
            public double dist_Y;
        };

        public class gerbv_knockout_t {
            public bool              firstInstance;
            public gerbv_knockout_type_t type;
            public gerbv_polarity_t      polarity;
            public double               lowerLeftX;
            public double               lowerLeftY;
            public double               width;
            public double               height;
            public double               border;
        };

        /*!  The structure used to keep track of RS274X layer groups */
        public class gerbv_layer_t {
            public gerbv_step_and_repeat_t stepAndRepeat; /*!< the current step and repeat group (refer to RS274X spec) */
            public gerbv_knockout_t        knockout;      /*!< the current knockout group (refer to RS274X spec) */
            public double                  rotation;      /*!< the current rotation around the origin */
            public gerbv_polarity_t        polarity;      /*!< the polarity of this layer */
            public string                  name;          /*!< the layer name (NULL for none) */
            public IntPtr                  next;          /*!< the next layer group in the array */
        };

        /*!  The structure used to keep track of RS274X state groups */
        public class gerbv_netstate_t {
            public gerbv_axis_select_t  axisSelect;  /*!< the AB to XY coordinate mapping (refer to RS274X spec) */
            public gerbv_mirror_state_t mirrorState; /*!< any mirroring around the X or Y axis */
            public gerbv_unit_t         unit;        /*!< the current length unit */
            public double              offsetA;     /*!< the offset along the A axis (usually this is the X axis) */
            public double              offsetB;     /*!< the offset along the B axis (usually this is the Y axis) */
            public double              scaleA;      /*!< the scale factor in the A axis (usually this is the X axis) */
            public double              scaleB;      /*!< the scale factor in the B axis (usually this is the Y axis) */
            public IntPtr             next;        /*!< the next state group in the array */
        };

        /*!  The structure used to hold a geometric entity (line/polygon/etc)*/
        public class gerbv_net_t {
            public double                 start_x;     /*!< the X coordinate of the start point */
            public double                 start_y;     /*!< the Y coordinate of the start point */
            public double                 stop_x;      /*!< the X coordinate of the end point */
            public double                 stop_y;      /*!< the Y coordinate of the end point */
            public gerbv_render_size_t    boundingBox; /*!< the bounding box containing this net (used for rendering optimizations) */
            public int                    aperture;    /*!< the index of the aperture used for this entity */
            public gerbv_aperture_state_t aperture_state; /*!< the state of the aperture tool (on/off/etc) */
            public gerbv_interpolation_t  interpolation;  /*!< the path interpolation method (linear/etc) */
            public gerbv_cirseg_t?        cirseg;         /*!< information for arc nets */
            public gerbv_net_t?           next;           /*!< the next net in the array */
            public string                 label;          /*!< a label string for this net */
            public gerbv_layer_t?         layer;          /*!< the RS274X layer this net belongs to */
            public gerbv_netstate_t?      state;          /*!< the RS274X state this net belongs to */
        };

        /*! Struct holding info about interpreting the Gerber files read
        *  e.g. leading zeros, etc.  */
        public class gerbv_format_t {
            public gerbv_omit_zeros_t omit_zeros;
            public gerbv_coordinate_t coordinate;
            public int                x_int;
            public int                x_dec;
            public int                y_int;
            public int                y_dec;
            public int                lim_seqno; /* Length limit for codes of sequence number */
            public int                lim_gf;    /* Length limit for codes of general function */
            public int                lim_pf;    /* Length limit for codes of plot function */
            public int                lim_mf;    /* Length limit for codes of miscellaneous function */
        };

        /*! Struct holding info about a particular image */
        public class gerbv_image_info_t {
            public string                     name;
            public gerbv_polarity_t           polarity;
            public double                     min_x; /* Always in inches */
            public double                     min_y;
            public double                     max_x;
            public double                     max_y;
            public double                     offsetA;
            public double                     offsetB;
            public gerbv_encoding_t           encoding;
            public double                     imageRotation;
            public gerbv_image_justify_type_t imageJustifyTypeA;
            public gerbv_image_justify_type_t imageJustifyTypeB;
            public double                     imageJustifyOffsetA;
            public double                     imageJustifyOffsetB;
            public double                     imageJustifyOffsetActualA;
            public double                     imageJustifyOffsetActualB;
            public string plotterFilm;

            /* Descriptive string for the type of file (rs274-x, drill, etc)
            * that this is
            */
            public string type;

            /* Attribute list that is used to hold all sorts of information
            * about how the layer is to be parsed.
            */
            public gerbv_HID_Attribute[]? attr_list;
            public int n_attr;
        };

        /*!  The structure used to hold a layer (RS274X, drill, or pick-and-place data) */
        public class gerbv_image_t {
            public gerbv_layertype_t     layertype;              /*!< the type of layer (RS274X, drill, or pick-and-place) */
            public gerbv_aperture_t[]    aperture;  /*[APERTURE_MAX]*/ /*!< an array with all apertures used */
            public gerbv_layer_t[]       layers;                 /*!< an array of all RS274X layers used (only used in RS274X types) */
            public gerbv_netstate_t[]    states;                 /*!< an array of all RS274X states used (only used in RS274X types) */
            public gerbv_amacro_t[]      amacro;                 /*!< an array of all macros used (only used in RS274X types) */
            public gerbv_format_t?       format;                 /*!< formatting info */
            public gerbv_image_info_t?   info;        /*!< miscellaneous info regarding the layer such as overall size, etc */
            public gerbv_net_t[]         netlist;     /*!< an array of all geometric entities in the layer */
            public gerbv_stats_t?        gerbv_stats; /*!< RS274X statistics for the layer */
            public gerbv_drill_stats_t?  drill_stats; /*!< Excellon drill statistics for the layer */
        };
    }
}  