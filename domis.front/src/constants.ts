export const USER_ROLES = {
  USER: "User",
  ADMIN: "Admin",
  VP1: "VP1",
  VP2: "VP2",
  VP3: "VP3",
  VP4: "VP4",
};

export function getUserRoleColor(userRole: string): string {
  switch (userRole) {
    case "User":
      return "bg-green-200";
    case "Admin":
      return "bg-gray-200";
    case "VP1":
    case "VP2":
    case "VP3":
    case "VP4":
      return "bg-blue-200";
    default:
      return "bg-gray-200";
  }
}
