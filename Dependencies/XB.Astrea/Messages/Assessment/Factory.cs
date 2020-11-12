using System;
using System.Collections.Generic;

namespace XB.Astrea.Client.Messages.Assessment
{
    public static class Factory
    {
        public static Request GetAssessmentRequest(string mt)
        {
            var request = new Request
            {
                OrderIdentity = "cd7z1Lja3",
                BasketIdentity = Guid.NewGuid(),
                Mt = mt,
                PaymentInstructions = new List<PaymentInstruction>(),
                Actor = new Actor(),
                Principal = new Principal(),
                TargetState = "requested",
                Tags = new Tags()
            };

            SetupPaymentInstruction(request.PaymentInstructions);
            SetupActor(request.Actor);
            SetupPrincipal(request.Principal);

            return request;
        }

        private static void SetupPaymentInstruction(List<PaymentInstruction> requestPaymentInstructions)
        {
            requestPaymentInstructions.Add(new PaymentInstruction()
            {
                //TODO: Should map to Mt103->{4:->:20:
                Identity = "cd7z1Lja3",
                PaymentType = "seb.payment.se.swift",
                RegisteringParty = new RegisteringParty()
                {
                    AuthId = "52041500480009",
                    SebId = "52041500480009"
                },
                RegistrationTime = DateTime.Now,
                InstructedDate = DateTime.Now,
                Amount = 199,
                Currency = "SEK",
                DebitAccount = new List<Account>()
                {
                    new Account()
                    {
                        Type = "iban",
                        BankIdentity = "ESSESESS",
                        Identity = "SE2750000000056970162486"
                    }
                },
                CreditAccount = new List<Account>()
                {
                    new Account()
                    {
                        Type = "iban",
                        BankIdentity = "ESSESESS",
                        Identity = "SE3550000000054910000003"
                    }
                },
                RemittanceInfo = new List<RemittanceInfo>()

                {

                },
                InstructionContext = new InstructionContext()
                {

                }
            });
        }

        private static void SetupActor(Actor requestActor)
        {

        }

        private static void SetupPrincipal(Principal requestPrincipal)
        {

        }
    }
}
