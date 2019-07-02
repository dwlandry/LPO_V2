using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel.DataAnnotations;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Piping
{
    public class PipingMetallurgy : BaseObject
    {
        
        public PipingMetallurgy(Session session) : base(session) { }
        
    }

    [DefaultClassOptions,CreatableItem(false), NavigationItem("Piping")]
    public class NominalPipeSize : BaseObject
    {
        
        public NominalPipeSize(Session session) : base(session) { }
        //public NominalPipeSize( Session session, double nominalSizeInInches, double outerDiameterInInches) : base(session)
        //{
        // 
        //    this.nominalSizeInInches = nominalSizeInInches;
        //    this.outerDiameterInInches = outerDiameterInInches;
        //}

        [Persistent("OuterDiameterInInches")]
        double outerDiameterInInches;
        [Persistent("NominalSizeInInches")]
        double nominalSizeInInches;

        [PersistentAlias("nominalSizeInInches")]
        [ReadOnly(true)]
        public double NominalSizeInInches
        {
            get => nominalSizeInInches;
            set => SetPropertyValue(nameof(NominalSizeInInches), ref nominalSizeInInches, value);
        }

        [PersistentAlias("outerDiameterInInches")]
        [ReadOnly(true)]
        public double OuterDiameterInInches
        {
            get => outerDiameterInInches;
            set => SetPropertyValue(nameof(OuterDiameterInInches), ref outerDiameterInInches, value);
        }

        public double OuterSurfaceAreaPerFootInFeetSquared
        {
            get
            {
                return OuterDiameterInInches / 12 * Math.PI;
            }
        }
    }
}