export function checkRole(user: UserState, requiredRole: string) {
  if (!user) {
    return false;
  }

  return user.userRole === requiredRole;
}
