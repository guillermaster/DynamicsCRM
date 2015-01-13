

namespace Efika.Crm.Entidades.ScoreEvaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ValidateField
    {
        #region vars
        private string mEntity;
        private string mSection;
        private string mField;
        private string mMessage;
        private string mDescription;
        private string mValue;
        #endregion
        #region constructors
        public ValidateField()
        {
        }

        public ValidateField(string entity, string section, string field, string message)
            : this(entity, section, field, message, string.Empty, string.Empty)
        {

        }


        public ValidateField(string entity, string section, string field, string message, string description, string value)
        {
            this.mEntity = entity;
            this.mSection = section;
            this.mField = field;
            this.mMessage = message;
            this.mDescription = description;
            this.mValue = value;
        }
        #endregion
        #region Properties

        public string Entity
        {
            get { return this.mEntity; }
            set { this.mEntity = value; }
        }

        public string Section
        {
            get { return this.mSection; }
            set { this.mSection = value; }
        }

        public string Field
        {
            get { return this.mField; }
            set { this.mField = value; }
        }

        public string Message
        {
            get { return this.mMessage; }
            set { this.mMessage = value; }
        }

        public string Description
        {
            get { return this.mDescription; }
            set { this.mDescription = value; }
        }

        public string Value
        {
            get { return this.mValue; }
            set { this.mValue = value; }
        }

        #endregion
    }
}
