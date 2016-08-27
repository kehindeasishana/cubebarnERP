namespace Core.Domain
{
    public abstract partial class BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual int Id { get; set; }

        //public virtual System.DateTime? CreatedOn { get; set; }
        //public virtual string CreatedBy { get; set; }
        //public virtual System.DateTime? ModifiedOn { get; set; }
        //public virtual string ModifiedBy { get; set; }
    }
}
