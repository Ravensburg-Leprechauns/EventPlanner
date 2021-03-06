﻿using ClubGrid.Enums;
using ClubGrid.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClubGrid.ViewModels
{
    public class EventParticipateViewModel : BaseViewModel
    {
        public Event CurrentEvent { get; set; }

        public ParticipationTypesEnum ParticipationType { get; set; }

        [DisplayName("Ich nehme als Spieler teil")]
        public bool IsPlayer { get; set; }
        public bool DisplayIsPlayer { get; set; }

        [DisplayName("Ich nehme als Coach teil")]
        public bool IsCoach { get; set; }
        public bool DisplayIsCoach { get; set; }

        [DisplayName("Ich nehme als Umpire teil")]
        public bool IsUmpire { get; set; }
        public bool DisplayIsUmpire { get; set; }

        [DisplayName("Ich nehme als Scorer teil")]
        public bool IsScorer { get; set; }
        public bool DisplayIsScorer { get; set; }

        [DisplayName("Ich kann fahren und habe (inklusive mir) Plätze")]
        public int HasSeats { get; set; }
        public bool DisplayHasSeats { get; set; }

        [StringLength(100, ErrorMessage = "Deine Nachricht kann maximal 100 Zeichen lang sein")]
        [DisplayName("Bemerkung")]
        public string Note { get; set; }
    }
}
