using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperHelperTests
    {
        private Mock<IHouseKeepersRepository> _repository;
        private DateTime _statementDate;
        private Mock<ISendEmail> _sendEmail;
        private Mock<ISaveStatement> _saveStatement;
        private Housekeeper _housekeeper;
        private Mock<IXtraMessageBox> _xtraMessageBox;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IHouseKeepersRepository>();
            _saveStatement = new Mock<ISaveStatement>();
            _sendEmail = new Mock<ISendEmail>();
            _xtraMessageBox = new Mock<IXtraMessageBox>();
            _statementDate = new DateTime(2020, 05, 18);
            
            _housekeeper = new Housekeeper
            {
                Oid = 1,
                FullName = "Test Baby",
                Email = "test@test.com",
                StatementEmailBody = "Test Email Body"
            };
        }
        
        [Test]
        public void SendStatementEmails_HouseKeeperHasEmail_SaveStatement()
        {
            _repository.Setup(r => r.GetHouseKeepers())
                .Returns(new List<Housekeeper> {_housekeeper}.AsQueryable());

            HousekeeperHelper.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _saveStatement.Verify(sS => sS.SaveStatementToPdf(_housekeeper.Oid, 
                _housekeeper.FullName, _statementDate));
        }
        
        [Test]
        public void SendStatementEmails_StatementIsSavedToFile_EmailFile()
        {
            _repository.Setup(r => r.GetHouseKeepers())
                .Returns(new List<Housekeeper> {_housekeeper}.AsQueryable());

            _saveStatement.Setup(s => s.SaveStatementToPdf(It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns("fileName");

            HousekeeperHelper.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _sendEmail.Verify(
                sE => sE.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, "fileName",
                    $"Sandpiper Statement {_statementDate:yyyy-MM} {_housekeeper.FullName}")
            );
        }
        
        [Test]
        public void SendStatementEmails_SendEmailReturnsException_ShowXtraBox()
        {
            _repository.Setup(r => r.GetHouseKeepers())
                .Returns(new List<Housekeeper> {_housekeeper}.AsQueryable());

            _saveStatement.Setup(s => s.SaveStatementToPdf(It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns("fileName");

            _sendEmail.Setup(sE => sE.EmailFile(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Throws<Exception>();

            HousekeeperHelper.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _xtraMessageBox.Verify(
                x => x.Show(new Exception().Message, 
                    $"Email failure: {_housekeeper.Email}",
                    MessageBoxButtons.OK)
            );
        }
    }
}