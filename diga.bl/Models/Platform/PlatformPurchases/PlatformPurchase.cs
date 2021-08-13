using diga.bl.Enums.PlatformPurchases;
using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace diga.bl.Models.Platform.PlatformPurchases
{
    public class PlatformPurchase : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        /// <summary>
        /// Reference transaction. For example tranfer is connected to payIn
        /// </summary>
        [ForeignKey("ParentPlatformPurchase")]
        public int? ParentId { get; set; }
        public DateTime Date { get; set; } // Дата оплаты
        /// <summary>
        /// Связь с внутренними покупками
        /// </summary>
        [ForeignKey("PaidFeatures")]
        public int? PaidFeatureId { get; set; }
        public double Price { get; set; }
        public double DebitedFromBalance { get; set; } //Сколько списано с баланса
        public double DebitedFromMoney { get; set; } //Сколько списано с карты
        /// <summary>
        /// Eupago or mangopay
        /// </summary>
        public PaymentProvider PaymentProvider { get; set; }
        /// <summary>
        /// Card or bank transfer or mbway
        /// </summary>
        public string PaymentMethod { get; set; }
        /// <summary>
        /// Связь с сущностью, которая была оплачена - только для Манго
        /// </summary>
        public int? EntityId { get; set; }
        public string EntityType { get; set; }
        public PaymentStatus Status { get; set; }
        /// <summary>
        /// External payment system identificator
        /// </summary>
        public string ExternalIdentificator { get; set; }

        public DateTime ActualDate { get; set; } // Фактичекая дата оплаты


        public PlatformPurchase ParentPlatformPurchase { get; set; }
    }
}
