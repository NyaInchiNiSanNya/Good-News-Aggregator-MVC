﻿namespace Business_Logic.ValidationRules
{
    internal interface ValidateErrors
    {
        protected const String NoName = "Введите имя";
        protected const String BadName = "Некорректное имя";

        protected const String NoEmail = "Введите почту";
        protected const String BadEmail = "Некорректный электронный адрес";

        protected const String NoPassword = "Введите пароль";
        protected const String BadPassword = 
            "Ваш пароль должен содержать символы верхнего и нижнего регистров, а так же цифры и спец. символ";

        protected const String BadConfirm = "Пароли не совпадают";

        protected const String EmailExist = "Пользователь с таким Email уже существует";


        protected const String BadTry = "Неправильный Email и(или) пароль";

    }
}
