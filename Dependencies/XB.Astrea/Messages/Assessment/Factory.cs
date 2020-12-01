using System;
using System.Collections.Generic;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.Assessment
{
    public static class Factory
    {
        public static Request GetAssessmentRequest(MT103SingleCustomerCreditTransferModel mt)
        {
            var request = new Request
            {
                OrderIdentity = Guid.NewGuid().ToString(),
                BasketIdentity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference,
                PaymentInstructions = new List<PaymentInstruction>(),
                Actor = new Actor(),
                Principal = new Principal(),
                TargetState = "requested",
                Tags = new Tags()
            };

            SetupPaymentInstruction(request.PaymentInstructions, mt);
            SetupActor(request.Actor, mt);
            SetupPrincipal(request.Principal, mt);

            return request;
        }

        private static void SetupPaymentInstruction(List<PaymentInstruction> requestPaymentInstructions, 
            MT103SingleCustomerCreditTransferModel mt)
        {
            requestPaymentInstructions.Add(new PaymentInstruction()
            {
                Identity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference,
                PaymentType = "seb.payment.se.swift." + mt.MT103SingleCustomerCreditTransferBlockText.Field23B,
                RegistrationTime = DateTime.Now,
                InstructedDate = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.Date,
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
            });
        }

        private static void SetupActor(Actor requestActor,
            MT103SingleCustomerCreditTransferModel mt)
        {

        }

        private static void SetupPrincipal(Principal requestPrincipal,
            MT103SingleCustomerCreditTransferModel mt)
        {

        }
    }
}
