using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services; // Предполагаем, что здесь находится ISecurityService

namespace DotLink.Application.Features.Users.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService; // Сервис для хэширования/проверки паролей

        // Предполагаем, что у вас зарегистрирован ISecurityService
        public ChangePasswordCommandHandler(IUserRepository userRepository, ISecurityService securityService)
        {
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                        ?? throw new ApplicationException("User not found.");

            // 1. Проверка старого пароля
            // Мы предполагаем, что у User Entity есть поле PasswordHash.
            // ISecurityService выполняет сравнение старого пароля с хэшем в БД.
            bool isOldPasswordCorrect = _securityService.VerifyPasswordHash(
                request.OldPassword,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isOldPasswordCorrect)
            {
                // Используем ApplicationException или специальный класс исключения, 
                // чтобы контроллер мог вернуть 401/400.
                throw new UnauthorizedAccessException("Invalid old password.");
            }

            // 2. Генерация нового хэша и соли
            _securityService.CreatePasswordHash(
                request.NewPassword,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            // 3. Обновление сущности пользователя
            user.UpdatePassword(passwordHash, passwordSalt); // Предполагаем, что этот метод есть в сущности User

            // 4. Сохранение изменений в БД
            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}