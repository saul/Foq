﻿/// Payment proxy example based on the code in Ben Hall's article -
/// Beginning to Mock with Rhino Mocks and MbUnit - Part 2 
/// http://aspalliance.com/1456_Beginning_to_Mock_with_Rhino_Mocks_and_MbUnit__Part_2.all
module ``Payment Proxy Example``

type IPaymentProcessor =
    abstract TakePayment: paymentId:int * customerId:int * amount:double -> bool

type PaymentProcessor(proxy:IPaymentProcessor) =
    member processor.TakePayment(paymentId, customerId, amount) =
        proxy.TakePayment(paymentId, customerId, amount)

open NUnit.Framework
open Foq

let [<Test>] ``take payment proxy should be called via payment processor(1)`` () =
    let proxy = 
        Mock<IPaymentProcessor>()
            .Setup(fun mock -> 
                <@ mock.TakePayment(any(),any(),any()) @>)
                .Returns(true)          
            .Create()

    let pp = PaymentProcessor(proxy)
    let result = pp.TakePayment(1, 1, 10.0)
    Assert.IsTrue(result)

    verify <@ proxy.TakePayment(1, 1, 10.00) @> once

let [<Test>] ``take payment proxy should be called via payment processor(2)`` () =
    let proxy = 
        Mock<IPaymentProcessor>
            .Method(fun mock -> <@ mock.TakePayment @>).Returns(true)
            
    let pp = PaymentProcessor(proxy)
    let result = pp.TakePayment(1, 1, 10.0)
    Assert.IsTrue(result)

    verify <@ proxy.TakePayment(1, 1, 10.00) @> once