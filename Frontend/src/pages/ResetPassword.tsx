import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { AlertCircle } from "lucide-react";
import { useAuthContext } from "@/AuthContext";

const ResetPassword = () => {
  const [newPassword, setNewPassword] = useState("");
  const [status, setStatus] = useState<{
    type: "success" | "error";
    message: string;
  } | null>(null);
  const [loading, setLoading] = useState(false);
  const { user, logout } = useAuthContext();
  const navigate = useNavigate();

  const handleReset = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setStatus(null);

    try {
      const res = await fetch(
        "https://localhost:64060/api/auth/reset-password",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ email: user.email, newPassword }),
        }
      );

      const data = await res.text();

      if (!res.ok) {
        setStatus({ type: "error", message: data || "Something went wrong." });
      } else {
        setStatus({ type: "success", message: data });

        // ✅ Log out the user after reset
        logout();

        setTimeout(() => navigate("/sign-in"), 2000);
      }
    } catch (err) {
      setStatus({ type: "error", message: "Network error. Please try again." });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen px-4">
      <div className="w-full max-w-md bg-white dark:bg-dm-dark shadow-md rounded-lg p-8">
        <h1 className="text-2xl font-bold text-center text-rich-black mb-6">
          Reset Password
        </h1>

        {status && (
          <Alert
            variant={status.type === "error" ? "destructive" : "default"}
            className="mb-4"
          >
            <AlertCircle className="h-5 w-5" />
            <AlertDescription>{status.message}</AlertDescription>
          </Alert>
        )}

        <form onSubmit={handleReset} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-1">Email</label>
            <Input
              type="email"
              value={user.email}
              disabled
              className="bg-gray-100 text-gray-500 cursor-not-allowed"
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-1">
              New Password
            </label>
            <Input
              type="password"
              placeholder="••••••••"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
            />
          </div>

          <Button
            type="submit"
            className="w-full bg-bice-blue hover:bg-bice-blue/90 text-white"
            disabled={loading}
          >
            {loading ? "Resetting..." : "Reset Password"}
          </Button>
        </form>
      </div>
    </div>
  );
};

export default ResetPassword;
