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
        private DateTime _statementDate = new DateTime(2020, 05, 18);
        private Mock<ISendEmail> _sendEmail;
        private Mock<ISaveStatement> _saveStatement;
        private Housekeeper _housekeeper;
        private Mock<IXtraMessageBox> _messageBox;
        private string _filename;

        [SetUp]
        public void Setup()
        {
            _housekeeper = new Housekeeper
            {
                Oid = 1,
                FullName = "Test Baby",
                Email = "test@test.com",
                StatementEmailBody = "Test Email Body"
            };
            
            _repository = new Mock<IHouseKeepersRepository>();
            _repository.Setup(r => r.GetHouseKeepers())
                .Returns(new List<Housekeeper> {_housekeeper}.AsQueryable());

            _filename = "fileName";
            _saveStatement = new Mock<ISaveStatement>();
            _saveStatement.Setup(s => s.SaveStatementToPdf(It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>()))
                .Returns(() => _filename); // Slow calling so I can manipulate the _filename value later
            
            _sendEmail = new Mock<ISendEmail>();
            _messageBox = new Mock<IXtraMessageBox>();
        }
        
        [Test]
        public void SendStatementEmails_HouseKeeperHasEmail_SaveStatement()
        {
            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _messageBox.Object);
            
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
                _repository.Object, _messageBox.Object);
            
            _saveStatement.Verify(sS => sS.SaveStatementToPdf(_housekeeper.Oid, 
                _housekeeper.FullName, _statementDate), Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_StatementIsSavedToFile_EmailFile()
        {
            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _messageBox.Object);
            
            _sendEmail.Verify(
                sE => sE.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, 
                    _filename,
                    It.IsAny<string>())
            );
        }
        
        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_StatementIsNotSavedToFile_FileShouldNotBeMailed(string badFileName)
        {
            _filename = badFileName;
            
            HousekeeperService.SendStatementEmails(_statementDate, _saveStatement.Object, _sendEmail.Object,
                _repository.Object, _messageBox.Object);
            
            _sendEmail.Verify(
                sE => sE.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, 
                    _filename,
                    It.IsAny<string>()),
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
                _repository.Object, _messageBox.Object);
            
            _messageBox.Verify(
                x => x.Show(It.IsAny<string>(),
                    It.IsAny<string>(),
                    MessageBoxButtons.OK)
            );
        }
    }
}