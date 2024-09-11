using System;

namespace Gerbvsharp {
    public static partial class Gerbv {

        /* ------------------------------------------------------------------ */
        /*! This is a wrapper which gets called from top level.  It
         *  does some initialization and pre-processing, and
         *  then calls gerber_parse_file_segment
         *  which processes the actual file.  Then it does final
         *  modifications to the image created.
         */

        public static partial gerbv_image_t parse_gerb(gerb_file_t fd, string directoryPath) {
            gerb_state_t? state = null;
            gerbv_image_t? image = null;
            gerbv_net_t? curr_net = null;
            gerbv_stats_t? stats;
            bool foundEOF = false;

            throw new NotImplementedException();

            ///*
            // * Create new state.  This is used locally to keep track
            // * of the photoplotter's state as the Gerber is read in.
            // */
            //state = g_new0(gerb_state_t, 1);
            //
            ///*
            // * Create new image.  This will be returned.
            // */
            //image = gerbv_create_image(image, "RS274-X (Gerber) File");
            //if (image == NULL)
            //    GERB_FATAL_ERROR("malloc image failed in %s()", __FUNCTION__);
            //curr_net = image->netlist;
            //image->layertype = GERBV_LAYERTYPE_RS274X;
            //image->gerbv_stats = gerbv_stats_new();
            //if (image->gerbv_stats == NULL)
            //    GERB_FATAL_ERROR("malloc gerbv_stats failed in %s()", __FUNCTION__);
            //
            //stats = image->gerbv_stats;
            //
            ///* set active layer and netstate to point to first default one created */
            //state->layer = image->layers;
            //state->state = image->states;
            //curr_net->layer = state->layer;
            //curr_net->state = state->state;
            //
            ///*
            // * Start parsing
            // */
            //dprintf("In %s(), starting to parse file...\n", __func__);
            //foundEOF = gerber_parse_file_segment(1, image, state, curr_net, stats, fd, directoryPath);
            //
            //if (!foundEOF) {
            //    gerbv_stats_printf(
            //        stats->error_list, GERBV_MESSAGE_ERROR, -1, _("Missing Gerber EOF code in file \"%s\""), fd->filename
            //    );
            //}
            //g_free(state);
            //
            //dprintf("               ... done parsing Gerber file\n");
            //gerber_update_any_running_knockout_measurements(image);
            //gerber_calculate_final_justify_effects(image);
            //
            //return image;
        } /* parse_gerb */
    }
}