using AuctionAPI.Concrete;
using AuctionAPI.Data;
using AuctionAPI.DTO;
using AuctionAPI.Entities;
using AuctionAPI.Repository;
using System.Text.RegularExpressions;

namespace AuctionAPI.Services
{
    public class UserService 
    {
        
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<List<User>> GetAllUser()
        {
            var userList = await _userRepository.GetAllAsync();
            return userList.ToList();
        }
        public static bool IsValidPassword(string password)
        {
            
            return Regex.IsMatch(password, @"^\d{6}$");
        }
        public static bool IsValidEmail(string email)
        {
            
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsValidPhoneNumber(string password)
        {
            
            return Regex.IsMatch(password, @"^\d{11}$");
        }
        public async Task AddBalance(UserOperationDTO userOperationDTO)
        {
            var userList = await _userRepository.GetAllAsync();
            var currentUser = userList.Where(x => x.Username == userOperationDTO.Username).FirstOrDefault();
            if(currentUser != null && currentUser.Password == userOperationDTO.Password) {
                await _userRepository.UpdateFieldAsync(currentUser.UserId, u => u.Balance, userOperationDTO.Balance);

                await _userRepository.SaveAsync();
            }
           
        }
        public async Task<User> CreateUser(UserOperationDTO userOperationDTO)
        {
            if (userOperationDTO.PhoneNumber != null && userOperationDTO.PhoneNumber.Count() > 0 &&
                userOperationDTO.Email != null && userOperationDTO.Email.Count() > 0 )
            {
                var userList = await _userRepository.GetAllAsync();
                var isForgetPass = userList.Any
                    (user => user.Email.Equals(userOperationDTO.Email, StringComparison.OrdinalIgnoreCase) && 
                    user.PhoneNumber == userOperationDTO.PhoneNumber &&
                    user.Username == userOperationDTO.Username);
                if (isForgetPass)
                {
                    if (!IsValidPassword(userOperationDTO.Password))
                    {
                        throw new Exception("The password must be exactly 6 digits long and consist only of numbers.");
                    }
                    
                    // PassForget
                    var currentUser = userList.Where(x => x.Username == userOperationDTO.Username).FirstOrDefault();
                    await _userRepository.UpdateFieldAsync(currentUser.UserId, u => u.Password, userOperationDTO.Password);
                   
                    await _userRepository.SaveAsync();
                }
                else if(userOperationDTO.IsNewUser)
                {
                    if (!IsValidPassword(userOperationDTO.Password))
                    {
                        throw new Exception("The password must be exactly 6 digits long and consist only of numbers.");
                    }
                    if (!IsValidEmail(userOperationDTO.Email))
                    {
                        throw new Exception("The email address must be in a valid format.");
                    }
                    if (userOperationDTO.Username.Count() > 50 && userOperationDTO.Firstname.Count() > 50 &&
                        userOperationDTO.Lastname.Count() > 50)
                    {
                        throw new Exception("Please take a shorter UserName.");
                    }
                    if (!IsValidPhoneNumber(userOperationDTO.PhoneNumber))
                    {
                        throw new Exception("The phone number must be exactly 11 digits long and consist only of numbers.");
                    }
                    //createUser
                    var user = new User()
                    {
                        Username = userOperationDTO.Username,
                        Password = userOperationDTO.Password,
                        PhoneNumber = userOperationDTO.PhoneNumber,
                        Email = userOperationDTO.Email,
                        FirstName = userOperationDTO.Firstname,
                        LastName = userOperationDTO.Lastname,
                    };
                    await _userRepository.AddAsync(user);
                    await _userRepository.SaveAsync();
                }
                else
                {
                    throw new Exception("Invalid Informations Check Infos Again");
                }
            }
            else
            {
                //login
                var userList = await _userRepository.GetAllAsync();
                var userHasExist = userList.Any(u => u.Username == userOperationDTO.Username &&
                u.Password == userOperationDTO.Password);
                if (!userHasExist)
                {
                    throw new Exception("Invalid username or password.");
                }
            }

            IEnumerable<User>? newUserList = await _userRepository.GetAllAsync();
            var activeUser = newUserList.Where(x => x.Username == userOperationDTO.Username && 
            x.Password == userOperationDTO.Password).FirstOrDefault();
            return activeUser;
        }
        private async Task<User> GetUserInfo(int userId)
        {
            var userInfo = await _userRepository.GetByIdAsync(userId);
            return userInfo;
        }

        public async Task UpdateUser(User user)
        {
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();
        }
         
        public async Task<User> GetUserWithId(int id)
        {
            var userInfo = await _userRepository.GetByIdAsync(id);
            return userInfo;
        }
    }
}
