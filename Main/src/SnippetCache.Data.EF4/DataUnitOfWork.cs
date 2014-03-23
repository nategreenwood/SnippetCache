using System;
using SnippetCache.Data.EF4.Model;

namespace SnippetCache.Data.EF4
{
    public class DataUnitOfWork : IDisposable
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly SnippetCacheEntitiesContainer _context;
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<Hyperlink> _hyperlinkRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<NotificationType> _notificationTypeRepository;
        private readonly IRepository<Snippet> _snippetRepository;
        private readonly IRepository<User> _userRepository;
        private bool _disposed;

        public DataUnitOfWork(SnippetCacheEntitiesContainer context)
        {
            _context = context;
            _languageRepository = new GenericRepository<Language>(_context);
            _commentRepository = new GenericRepository<Comment>(_context);
            _userRepository = new GenericRepository<User>(_context);
            _notificationRepository = new GenericRepository<Notification>(_context);
            _notificationTypeRepository = new GenericRepository<NotificationType>(_context);
            _fileRepository = new GenericRepository<File>(_context);
            _hyperlinkRepository = new GenericRepository<Hyperlink>(_context);
            _snippetRepository = new GenericRepository<Snippet>(_context);
        }

        public IRepository<Language> LanguageRepository
        {
            get { return _languageRepository; }
        }

        public IRepository<Comment> CommentRepository
        {
            get { return _commentRepository; }
        }

        public IRepository<User> UserRepository
        {
            get { return _userRepository; }
        }

        public IRepository<Notification> NotificationRepository
        {
            get { return _notificationRepository; }
        }

        public IRepository<NotificationType> NotificationTypeRepository
        {
            get { return _notificationTypeRepository; }
        }

        public IRepository<File> FileRepository
        {
            get { return _fileRepository; }
        }

        public IRepository<Hyperlink> HyperLinkRepository
        {
            get { return _hyperlinkRepository; }
        }

        public IRepository<Snippet> SnippetRepository
        {
            get { return _snippetRepository; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}