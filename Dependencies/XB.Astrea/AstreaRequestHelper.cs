using System;
using System.Collections.Generic;
using System.Text;

namespace XB.Astrea.Client
{
    public static class AstreaRequestHelper
    {
        public static AstreaRequest ParseMtToAstreaRequest(string mt)
        {
            var request = new AstreaRequest
            {
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
                Identity = Guid.NewGuid().ToString(),
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
                DebitAccount = new List<DebitAccount>()
                {
                    new DebitAccount()
                    {
                        Type = "iban",
                        BankIdentity = "ESSESESS",
                        Identity = "SE2750000000056970162486"
                    }
                },
                CreditAccount = new List<CreditAccount>()
                {

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
