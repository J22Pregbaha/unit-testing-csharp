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
            
            _repository.Setup(r => r.GetHouseKeepers())
                .Returns(new List<Housekeeper> {_housekeeper}.AsQueryable());
            
            _saveStatement.Setup(s => s.SaveStatementToPdf(It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns("fileName");
        }
        
        [Test]
        public void SendStatementEmails_HouseKeeperHasEmail_SaveStatement()
        {
            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _saveStatement.Verify(sS => sS.SaveStatementToPdf(_housekeeper.Oid, 
                _housekeeper.FullName, _statementDate));
        }
        
        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_HouseKeeperDoesNotHaveEmail_ShouldNotSaveStatement(string badEmail)
        {
            _housekeeper.Email = badEmail;

            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _saveStatement.Verify(sS => sS.SaveStatementToPdf(_housekeeper.Oid, 
                _housekeeper.FullName, _statementDate), Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_StatementIsSavedToFile_EmailFile()
        {
            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _sendEmail.Verify(
                sE => sE.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, "fileName",
                    $"Sandpiper Statement {_statementDate:yyyy-MM} {_housekeeper.FullName}")
            );
        }
        
        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_StatementIsNotSavedToFile_FileShouldNotBeMailed(string badFileName)
        {
            _saveStatement.Setup(s => s.SaveStatementToPdf(It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns(badFileName);
            
            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _sendEmail.Verify(
                sE => sE.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, "fileName",
                    $"Sandpiper Statement {_statementDate:yyyy-MM} {_housekeeper.FullName}"),
                Times.Never
            );
        }
        
        [Test]
        public void SendStatementEmails_SendEmailReturnsException_ShowXtraBox()
        {
            _sendEmail.Setup(sE => sE.EmailFile(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Throws<Exception>();

            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _xtraMessageBox.Object);
            
            _xtraMessageBox.Verify(
                x => x.Show(new Exception().Message, 
                    $"Email failure: {_housekeeper.Email}",
                    MessageBoxButtons.OK)
            );
        }
    }
}