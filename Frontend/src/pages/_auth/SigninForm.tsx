import { useState } from "react";
import { Link } from "react-router-dom";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { AlertCircle, Loader2 } from "lucide-react";
import { useAuthContext } from "@/AuthContext";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

const SignInValidation = z.object({
  email: z.string().email("Please enter a valid email address"),
  password: z.string().min(6, "Password must be at least 6 characters"),
});

const SigninForm = () => {
  const { login, isLoading, error, clearError } = useAuthContext();
  const [showPassword, setShowPassword] = useState(false);

  const form = useForm<z.infer<typeof SignInValidation>>({
    resolver: zodResolver(SignInValidation),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (values: z.infer<typeof SignInValidation>) => {
    clearError(); // Clear any previous errors
    const { success } = await login(values.email, values.password);

    if (!success) {
      // Error message is already set in the AuthContext
      // No need to set it here as it will be displayed from the context's error state
      return;
    }
  };

  const handleDemoLogin = (role: "teacher" | "student") => {
    clearError();
    const demoCredentials = {
      teacher: { email: "teacher@example.com", password: "12345678" },
      student: { email: "student@example.com", password: "12345678" },
    };
    form.setValue("email", demoCredentials[role].email);
    form.setValue("password", demoCredentials[role].password);

    // Auto-submit the form after setting demo credentials
    form.handleSubmit(onSubmit)();
  };

  return (
    <div className="flex flex-1 flex-col bg-light-blue items-center justify-center p-4 sm:p-6 lg:p-8">
      <div className="w-full max-w-md space-y-8">
        <div className="text-center text-rich-black">
          <h1 className="text-2xl font-bold tracking-tight">Welcome back</h1>
          <p className="mt-2 text-md">Sign in to your account to continue</p>
        </div>

        {/* Error message from AuthContext */}
        {error && (
          <Alert
            variant="destructive"
            className="border-choco-cosmos bg-choco-cosmos/10"
          >
            <AlertCircle className="h-4 w-4 text-choco-cosmos" />
            <AlertDescription className="text-choco-cosmos">
              {error}
            </AlertDescription>
          </Alert>
        )}

        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="mt-8 space-y-6"
            noValidate
          >
            <div className="space-y-4 text-rich-black">
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Email address</FormLabel>
                    <FormControl>
                      <Input
                        type="email"
                        autoComplete="email"
                        disabled={isLoading}
                        placeholder="you@example.com"
                        className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <div className="flex items-center justify-between text-rich-black">
                      <FormLabel>Password</FormLabel>
                      <Button
                        type="button"
                        variant="link"
                        size="sm"
                        className="h-auto px-0 text-sm text-bice-blue"
                        onClick={() => setShowPassword(!showPassword)}
                      >
                        {showPassword ? "Hide" : "Show"}
                      </Button>
                    </div>
                    <FormControl>
                      <Input
                        type={showPassword ? "text" : "password"}
                        autoComplete="current-password"
                        disabled={isLoading}
                        placeholder="••••••••"
                        className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />
            </div>

            <div className="flex justify-center">
              <Button
                type="submit"
                disabled={isLoading}
                className="w-1/2 bg-bice-blue hover:bg-blue-950 text-white"
              >
                {isLoading ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Signing in...
                  </>
                ) : (
                  "Sign in"
                )}
              </Button>
            </div>
          </form>
        </Form>

        <div className="mt-6">
          <div className="relative">
            <div className="absolute inset-0 flex items-center">
              <div className="w-full border-t border-rich-black/20" />
            </div>
            <div className="relative flex justify-center text-xs uppercase">
              <span className="bg-light-blue px-2 text-rich-black">
                Or try a demo account
              </span>
            </div>
          </div>

          <div className="mt-6 grid grid-cols-2 gap-3">
            <Button
              type="button"
              variant="outline"
              onClick={() => handleDemoLogin("teacher")}
              disabled={isLoading}
              className="border-bice-blue bg-light-blue text-bice-blue hover:bg-bice-blue hover:text-white"
            >
              Teacher Demo
            </Button>
            <Button
              type="button"
              variant="outline"
              onClick={() => handleDemoLogin("student")}
              disabled={isLoading}
              className="border-bice-blue bg-light-blue text-bice-blue hover:bg-bice-blue hover:text-white"
            >
              Student Demo
            </Button>
          </div>
        </div>

        <div className="flex items-center justify-center pt-4">
          <div className="text-sm text-rich-black/70 ml-4">
            Don't have an account?{" "}
            <Button
              asChild
              variant="link"
              size="sm"
              className="px-0 text-bice-blue hover:text-bice-blue/80"
            >
              <Link to="/sign-up" onClick={clearError}>
                Sign up
              </Link>
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SigninForm;
