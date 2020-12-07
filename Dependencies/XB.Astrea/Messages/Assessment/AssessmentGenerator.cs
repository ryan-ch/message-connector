using System;
using System.Collections.Generic;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.Assessment
{
    public static class AssessmentGenerator
    {
        public static AssessmentRequest GetAssessmentRequest(MT103SingleCustomerCreditTransferModel mt)
        {
            var request = new AssessmentRequest
            {
                OrderIdentity = Guid.NewGuid().ToString(),
                BasketIdentity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference,
                PaymentInstructions = SetupPaymentInstruction(mt),
                Actor = SetupActor(mt),
                Principal = SetupPrincipal(mt),
                TargetState = "requested",
                Tags = new Tags()
            };

            return request;
        }

        private static List<PaymentInstruction> SetupPaymentInstruction(MT103SingleCustomerCreditTransferModel mt)
        {
            var paymentInstructionList = new List<PaymentInstruction>
            {
                new PaymentInstruction()
                {
                    Identity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference,
                    PaymentType = "seb.payment.se.swift." + mt.MT103SingleCustomerCreditTransferBlockText.Field23B,
                    RegistrationTime = DateTime.Now,
                    InstructedDate = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.ValueDate,
                    Amount = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.InterbankSettledAmount,
                    Currency = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.Currency,
                    DebitAccount = new List<Account>()
                {
                    new Account()
                    {
                        //TODO: check if length is greater then 11 and if first two are alphabetic
                        Type = "iban",
                        Identity = "SE2750000000056970162486"
                    }
                },
                    CreditAccount = new List<Account>()
                {
                    new Account()
                    {
                        //TODO: check if length is greater then 11 and if first two are alphabetic
                        Type = "iban",
                        Identity = "SE3550000000054910000003"
                    }
                },
                    RemittanceInfo = new List<RemittanceInfo>()
                    {
                    },
                    InstructionContext = new InstructionContext()
                    {
                    },
                    RegisteringParty = new RegisteringParty()
                    {
                    },
                }
            };

            return paymentInstructionList;
        }

        private static Actor SetupActor(MT103SingleCustomerCreditTransferModel mt)
        {
            //Todo:
            throw new NotImplementedException();
        }

        private static Principal SetupPrincipal(MT103SingleCustomerCreditTransferModel mt)
        {
            //Todo:
            throw new NotImplementedException();
        }
    }
}
